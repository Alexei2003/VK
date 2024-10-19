using DataSet;
using System.Drawing;
using System.Drawing.Imaging;

namespace NeuralNetwork
{
    public static class NeuralNetwork
    {
        public static string NeuralNetworkResult(Bitmap image, float percentOriginalTag)
        {
            string resulTag;
            image = DataSetPhoto.ChangeResolution224224(image);

            // Преобразуйте изображение в байты
            byte[] imageBytes;
            using (var stream = new MemoryStream())
            {
                // Сохраните изображение в том же формате, в котором оно находится в буфере обмена
                image.Save(stream, ImageFormat.Jpeg);
                imageBytes = stream.ToArray();
            }

            // Подача данных в модель
            var sampleData = new ComputerVision.ModelInput()
            {
                ImageSource = imageBytes,
            };

            //Load model and predict output
            var resultArts = ComputerVision.Predict(sampleData);

            var scores = resultArts.Score;

            resulTag = resultArts.PredictedLabel;
            if (scores.Max() < percentOriginalTag || resulTag.Contains("#Original_"))
            {
                resulTag = "#Original";
            };

            return resulTag;
        }
    }
}
