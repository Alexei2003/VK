using DataSet;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace NeuralNetwork
{
    public static class NeuralNetwork
    {
        private static readonly InferenceSession _session;
        private static readonly string _inputName;

        // Статическая модель и метки классов
        private static readonly string[] _labels;

        // Статический конструктор для инициализации
        static NeuralNetwork()
        {
            // Загружаем модель 
            _session = new InferenceSession("model.onnx");
            _inputName = _session.InputMetadata.Keys.First();

            // Загружаем метки классов
            string labelFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "labels.txt");
            if (File.Exists(labelFilePath))
            {
                _labels = File.ReadAllLines(labelFilePath);
            }
            else
            {
                _labels = ["#unknown"]; // На случай, если файл отсутствует
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

        public static string[] NeuralNetworkResultKTop(Image<Rgb24> image, int kTop = 15)
        {
            image = DataSetImage.ChangeResolution224x224(image);

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
                labels[i] = new Label(_labels[i], outputArr[i]);
            }

            labels = labels.OrderByDescending(value => value.Value).ToArray();
            var resulTagsArr = new string[kTop];

            for (var i = 0; i < kTop; i++)
            {
                ref var label = ref labels[i];
                if (label.Name == "#nsfw")
                {
                    for (var n = 0; n < kTop; n++)
                    {
                        resulTagsArr[n] = "#nsfw";
                    }
                    break;
                }
                resulTagsArr[i] = label.Name;
            }

            return resulTagsArr;
        }

        public static string NeuralNetworkResult(Image<Rgb24> image)
        {
            var arr = NeuralNetworkResultKTop(image, 1);

            return arr[0];
        }


        public static DenseTensor<float> ImageToTensor(Image<Rgb24> bitmap)
        {
            const int batchSize = 1;
            const int height = 224;
            const int width = 224;
            const int channels = 3;

            var tensor = new DenseTensor<float>([batchSize, height, width, channels]);

            for (int b = 0; b < batchSize; b++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        tensor[b, y, x, 0] = bitmap[x, y].B / 255f;
                        tensor[b, y, x, 1] = bitmap[x, y].G / 255f;
                        tensor[b, y, x, 2] = bitmap[x, y].R / 255f;
                    }
                }
            }

            return tensor;
        }
    }
}