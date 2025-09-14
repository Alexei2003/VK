using DataSet;

using Other;

using SixLabors.ImageSharp.PixelFormats;

namespace DownloaderDataSetPhoto.Downloaders
{
    public static class Downloader
    {
        public static void DownloadPhoto(HttpClient httpClient, Uri url, string currentTag, string fileName)
        {
            Directory.CreateDirectory("DATA_SET");
            if (!ImageTransfer.DownloadImageAsync(httpClient, url, $"DATA_SET\\{fileName}.jpg").Result)
            {
                return;
            }
            using var image = SixLabors.ImageSharp.Image.Load<Rgb24>($"DATA_SET\\{fileName}.jpg");

            var pathDir = "DATA_SET\\" + currentTag;
            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }

            var getTag = NeuralNetwork.NeuralNetworkWorker.NeuralNetworkResult(image, 0);
            if (getTag == currentTag)
            {
                return;
            }

            DataSetImage.Save(image, currentTag);
        }
    }
}
