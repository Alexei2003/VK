using System.Numerics;

using DataSet;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

using Other;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace NeuralNetwork
{
    public static class NeuralNetworkWorker
    {
        private const string _path = "";

        public const int MAX_SESSIONS_GPU = 4;

        private static readonly Session[] _sessionArr;

        private static readonly string _inputName;

        // Статическая модель и метки классов
        public static string[] Labels { get; private set; }

        private sealed class Session
        {
            public bool IsFirst { get; set; } = true;
            public int GPUID { get; set; } = -1;

            public InferenceSession Inference { get; set; }

            public Session(InferenceSession inference, int gpuid)
            {
                GPUID = gpuid;
                Inference = inference;
            }
        }

        // Статический конструктор для инициализации
        static NeuralNetworkWorker()
        {
            // Загружаем метки классов
            string labelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "E:\\WPS\\CommonData\\Model\\labels.txt");
            if (File.Exists(labelFilePath))
            {
                Labels = File.ReadAllLines(labelFilePath);
            }
            else
            {
                Labels = ["#unknown"]; // На случай, если файл отсутствует
            }

            var sessionList = new List<Session>();

            // Загружаем модель
            var id = 0;
            while (true)
            {
                for (var i = 0; i < MAX_SESSIONS_GPU; i++)
                {
                    var (inferenceSession, isCPU) = CreateInferenceSession(id);
                    if (isCPU)
                    {
                        if (sessionList.Count == 0)
                        {
                            sessionList.Add(new Session(inferenceSession, -1));
                        }
                        id = -1;
                        break;
                    }
                    else
                    {
                        sessionList.Add(new Session(inferenceSession, id));
                    }
                }
                if (id == -1)
                {
                    break;
                }
                id++;
            }

            _sessionArr = [.. sessionList];
            _inputName = _sessionArr[0].Inference.InputMetadata.Keys.First();
        }

        private static (InferenceSession, bool) CreateInferenceSession(int id = -1)
        {
            var options = new SessionOptions();
            var isCPU = true;
#if WINDOWS
            if (id != -1)
            {
                // Пытаемся использовать DirectML (работает через DirectX 12)
                try
                {
                    options.AppendExecutionProvider_DML(id);
                    isCPU = false;
                }
                catch { }
            }
#endif
            return (new InferenceSession(Path.Combine(_path, "E:\\WPS\\CommonData\\Model\\model.onnx"), options), isCPU);
        }

        public struct Label
        {
            public string Name;
            public float Value;

            public Label(string name, float value)
            {
                Name = name;
                Value = value;
            }
        }

        public static string NeuralNetworkResult(Image<Rgb24> imageOriginal, int threadIndex)
        {
            var arr = NeuralNetworkResultKTopCount(imageOriginal, threadIndex, 1);

            return arr[0];
        }

        public static string[] NeuralNetworkResultKTopPercent(Image<Rgb24> imageOriginal, int threadIndex, double percent = 0.1)
        {
            var kTop = int.Max((int)(Labels.Length * percent), 1);

            var arr = NeuralNetworkResultKTopCount(imageOriginal, threadIndex, kTop);
            return arr;
        }

        public static string[] NeuralNetworkResultKTopCount(Image<Rgb24> imageOriginal, int threadIndex, int kTop = 10)
        {
            var labels = NeuralNetworkBaseResult(imageOriginal, threadIndex);
            if (labels == null)
            {
                return ["#error"];
            }

            var resultTagsArr = labels.OrderByDescending(l => l.Value).Take(kTop).Select(l => l.Name);
            if (resultTagsArr.Contains("#nsfw"))
            {
                resultTagsArr = ["#nsfw"];
            }
            if (resultTagsArr.Contains("#bad_drawing"))
            {
                resultTagsArr = ["#bad_drawing"];
            }
            return [.. resultTagsArr];
        }

        public static Label[] NeuralNetworkResultKTopCountAndPercent(Image<Rgb24> imageOriginal, int threadIndex, int kTop = 10)
        {
            var labels = NeuralNetworkBaseResult(imageOriginal, threadIndex);
            if (labels == null)
            {
                return [new Label("#error", 1)];
            }

            // 1. Считаем экспоненты логитов
            var exps = labels.Select(l => Math.Exp(l.Value)).ToArray();

            // 2. Суммируем экспоненты
            var sumExp = exps.Sum();

            // 3. Применяем softmax к каждому значению
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i].Value = (float)(exps[i] / sumExp) * 100;
            }

            // 4. Берём топ k по убыванию вероятности
            var resultTagsArr = labels.OrderByDescending(l => l.Value).Take(kTop);

            return [.. resultTagsArr];
        }

        private static Label[]? NeuralNetworkBaseResult(Image<Rgb24> imageOriginal, int threadIndex)
        {
            using var image = DataSetImage.ChangeResolution224x224(imageOriginal);

            var inputTensor = ImageToTensor(image);

            var input = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputName, inputTensor)
            };

            var session = _sessionArr[threadIndex % _sessionArr.Length];

            float[] outputArr;
            lock (session)
            {
                try
                {
                    if (session.IsFirst == true)
                    {
                        lock (_sessionArr)
                        {
                            using var results = session.Inference.Run(input);
                            outputArr = [.. results[0].AsEnumerable<float>()];
                        }
                        session.IsFirst = false;
                    }
                    else
                    {
                        using var results = session.Inference.Run(input);
                        outputArr = [.. results[0].AsEnumerable<float>()];
                    }
                }
                catch (Exception ex)
                {
                    Logs.WriteException(ex);

                    session.Inference.Dispose();
                    session.IsFirst = true;
                    var (inferenceSession, isCPU) = CreateInferenceSession(session.GPUID);
                    session.Inference = inferenceSession;

                    return null;
                }
            }

            var labels = new Label[outputArr.Length];

            for (var i = 0; i < outputArr.Length; i++)
            {
                labels[i] = new Label(Labels[i], outputArr[i]);
            }

            return labels;
        }

        public static DenseTensor<float> ImageToTensor(Image<Rgb24> bitmap)
        {
            const int batchSize = 1;
            const int height = 224;
            const int width = 224;
            const int channels = 3;

            var tensor = new DenseTensor<float>([batchSize, channels, height, width]);

            var constVect = new Vector3(255f);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixel = bitmap[x, y];
                    var vect = new Vector3(pixel.R, pixel.G, pixel.B);
                    vect = vect / constVect;
                    tensor[0, 0, y, x] = vect[0];
                    tensor[0, 1, y, x] = vect[1];
                    tensor[0, 2, y, x] = vect[2];
                }
            }

            return tensor;
        }
    }
}