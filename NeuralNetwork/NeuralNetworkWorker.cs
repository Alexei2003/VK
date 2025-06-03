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
        public const int MAX_SESSIONS_GPU = 4;

        private static readonly Session[] _sessionArr;

        private static readonly string _inputName;

        // Статическая модель и метки классов
        public static string[] Labels { get; private set; }

        private sealed class Session
        {
            public bool IsBusy { get; set; } = false;
            public bool IsFirst { get; set; } = true;

            public InferenceSession Inference { get; set; }

            public Session(InferenceSession inference)
            {
                Inference = inference;
            }
        }



        // Статический конструктор для инициализации
        static NeuralNetworkWorker()
        {
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
                        if(sessionList.Count == 0)
                        {
                            sessionList.Add(new Session(inferenceSession));
                        }
                        id = -1;
                        break;
                    }
                    else
                    {
                        sessionList.Add(new Session(inferenceSession));
                    }
                }
                if(id == -1)
                {
                    break;
                }
                id++;
            }


            _sessionArr = [.. sessionList];
            _inputName = _sessionArr[0].Inference.InputMetadata.Keys.First();

            // Загружаем метки классов
            string labelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "labels.txt");
            if (File.Exists(labelFilePath))
            {
                Labels = File.ReadAllLines(labelFilePath);
            }
            else
            {
                Labels = ["#unknown"]; // На случай, если файл отсутствует
            }
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
            return (new InferenceSession("model.onnx", options), isCPU);
        }

        private struct Label
        {
            public string Name;
            public float Value;

            public Label(string name, float value)
            {
                Name = name;
                Value = value;
            }
        }

        public static string NeuralNetworkResult(Image<Rgb24> imageOriginal)
        {
            var arr = NeuralNetworkResultKTopCount(imageOriginal, 1);

            return arr[0];
        }

        public static string[] NeuralNetworkResultKTopPercent(Image<Rgb24> imageOriginal, double percent = 0.10)
        {
            var kTop = int.Max((int)(Labels.Length * percent), 1);

            var arr = NeuralNetworkResultKTopCount(imageOriginal, kTop);
            return arr;
        }

        private static string[] NeuralNetworkResultKTopCount(Image<Rgb24> imageOriginal, int kTop = 15)
        {
            using var image = DataSetImage.ChangeResolution224x224(imageOriginal);

            var inputTensor = ImageToTensor(image);

            var input = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputName, inputTensor)
            };

            Session? session;

            while (true)
            {
                session = _sessionArr.FirstOrDefault(s => s.IsBusy == false);

                if (session != null)
                {
                    session.IsBusy = true;
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }


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

                    var id = -1;
                    if (_sessionArr.Length > 1)
                    {
                        int index = Array.IndexOf(_sessionArr, session);
                        id = index / MAX_SESSIONS_GPU;
                    }
                    session.Inference.Dispose();
                    session.IsFirst = true;
                    var (inferenceSession, isCPU) = CreateInferenceSession(id);
                    session.Inference = inferenceSession;
                    session.IsBusy = false;

                    return ["#error"];
                }
            }
            session.IsBusy = false;

            var labels = new Label[outputArr.Length];

            for (var i = 0; i < outputArr.Length; i++)
            {
                labels[i] = new Label(Labels[i], outputArr[i]);
            }

            var resulTagsArr = labels.OrderByDescending(l => l.Value).Take(kTop).Select(l => l.Name);

            if (resulTagsArr.Contains("#nsfw"))
            {
                resulTagsArr = ["#nsfw"];
            }

            if (resulTagsArr.Contains("#bad_drawing"))
            {
                resulTagsArr = ["#bad_drawing"];
            }

            return [.. resulTagsArr];
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