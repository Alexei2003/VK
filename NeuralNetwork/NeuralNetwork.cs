using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

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
                _labels = ["#Unknown"]; // На случай, если файл отсутствует
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

        public static string[] NeuralNetworkResult(Bitmap image,  int kTop = 1)
        {
            var inputTensor = ImageToTensor(image);

            var input = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputName, inputTensor)
            };

            using var results = _session.Run(input);

            // Получаем результат
            var outputArr = results[0].AsEnumerable<float>().ToArray();

            var labels = new Label[outputArr.Length];

            for(var i = 0; i < outputArr.Length; i++)
            {
                labels[i] = new Label(_labels[i], outputArr[i]);
            }

            labels = labels.OrderByDescending(value => value.Value).ToArray();
            var resulTagsArr = new string[kTop];

            for (var i = 0; i < kTop; i++)
            {
                // предыдущих 3х
                float percentOriginalTag = labels.Skip(i+1).Take(3).Sum(value => value.Value);

                if (labels[i].Value < percentOriginalTag)
                {
                    resulTagsArr[i] = "#Original";
                }
            }

            return resulTagsArr;
        }

        public unsafe static DenseTensor<float> ImageToTensor(Bitmap bitmap)
        {
            const int batchSize = 1;
            const int height = 224;
            const int width = 224;
            const int channels = 3;

            var tensor = new DenseTensor<float>([batchSize, height, width, channels]);

            var rgbBitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
            var ptr = rgbBitmapData.Scan0;
            var rgbBitmap = (byte*)ptr;

            for (int b = 0; b < batchSize; b++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int c = channels - 1; c != -1; c--)
                        {
                            var yi = y < bitmap.Height ? y : bitmap.Height - 1;
                            var xi = x < bitmap.Width ? x : bitmap.Width - 1;
                            tensor[b, y, x, c] = rgbBitmap[yi * rgbBitmapData.Stride + xi * channels + c] / 255f;
                        }
                    }
                }
            }

            bitmap.UnlockBits(rgbBitmapData);

            return tensor;
        }
    }
}
