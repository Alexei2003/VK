using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System.Drawing;
using System.Drawing.Imaging;

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
            var modelPath = "model.onnx";
            _session = new InferenceSession(modelPath);
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

        public static string NeuralNetworkResult(Bitmap image, float percentOriginalTag)
        {
            string resulTag = "#Error";

            var inputTensor = ImageToTensor(image);

            var input = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputName, inputTensor)
            };

            IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results;
            lock (_session)
            {
                results = _session.Run(input);
            }

            // Получаем результат
            var outputArr = results[0].AsEnumerable<Float16>().ToArray();

            // Определяем метку с наибольшей вероятностью
            if (outputArr != null)
            {
                int maxIndex = Array.IndexOf(outputArr, outputArr.Max());
                resulTag = _labels.Length > maxIndex ? _labels[maxIndex] : "#Unknown"; ; // Получаем имя класса по индексу

                Int64 sum = 0;
                foreach (var elem in outputArr)
                {
                    sum += elem.value;
                }
                var percentMaxTag = (outputArr[maxIndex].value * 1f) / sum;

                // Логика обработки метки
                if (percentMaxTag < percentOriginalTag || resulTag.Contains("#Original_"))
                {
                    resulTag = "#Original";
                }

                if (resulTag.Contains("#NSFW_"))
                {
                    resulTag = "#NSFW";
                }
            }

            results.Dispose();
            return resulTag;
        }

        public unsafe static DenseTensor<float> ImageToTensor(Bitmap bitmap)
        {
            int batchSize = 1;
            int height = 224;
            int width = 224;
            int channels = 3;

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
                        for (int c = 0; c < channels; c++)
                        {
                            var yi = y < bitmap.Height ? y : bitmap.Height;
                            var xi = x < bitmap.Width ? x : bitmap.Width;
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
