using System.Numerics;

using DataSet;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace NeuralNetwork
{
    public static class NeuralNetworkWorker
    {
        private static InferenceSession _session;
        private static string _inputName;

        // Статическая модель и метки классов
        public static string[] Labels { get; private set; }


        // Статический конструктор для инициализации
        static NeuralNetworkWorker()
        {
            // Загружаем модель 
            SessionOptions options = new();
            try
            {
                // Пытаемся использовать DirectML (работает через DirectX 12)
                options.AppendExecutionProvider_DML();
                _session = new InferenceSession("model.onnx", options);
            }
            catch
            {
                // Fallback на CPU
                _session = new InferenceSession("model.onnx");
            }
            _inputName = _session.InputMetadata.Keys.First();

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

        public static string[] NeuralNetworkResultKTopCount(Image<Rgb24> imageOriginal, int kTop = 15)
        {
            using var image = DataSetImage.ChangeResolution224x224(imageOriginal);

            var inputTensor = ImageToTensor(image);

            var input = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputName, inputTensor)
            };

            using var results = _session.Run(input);

            // Получаем результат
            var outputArr = results[0].AsEnumerable<float>().ToArray();

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

            return [.. resulTagsArr];
        }




        public static DenseTensor<float> ImageToTensor(Image<Rgb24> bitmap)
        {
            const int batchSize = 1;
            const int height = 224;
            const int width = 224;
            const int channels = 3;

            var tensor = new DenseTensor<float>([batchSize, height, width, channels]);

            var constVect = new Vector3(255f);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var pixel = bitmap[x, y];
                    var vect = new Vector3(pixel.B, pixel.G, pixel.R);
                    vect = vect / constVect;
                    tensor[0, y, x, 0] = vect[0];
                    tensor[0, y, x, 1] = vect[1];
                    tensor[0, y, x, 2] = vect[2];
                }
            }

            return tensor;
        }
    }
}