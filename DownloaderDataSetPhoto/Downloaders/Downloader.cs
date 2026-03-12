using DataSet;

using Other;
using Other.Tags;

using SixLabors.ImageSharp.PixelFormats;

namespace DownloaderDataSetPhoto.Downloaders
{
    public static class Downloader
    {
        public static void DownloadPhoto(HttpClient httpClient, Uri url, string currentTag, string fileName)
        {
            Directory.CreateDirectory("DATA_SET");
            if (ImageTransfer.DownloadImageAsync(httpClient, url, $"DATA_SET\\{fileName}.jpg").Result)
            {
                using var image = SixLabors.ImageSharp.Image.Load<Rgb24>($"DATA_SET\\{fileName}.jpg");

                var pathDir = "DATA_SET\\" + currentTag;
                if (!Directory.Exists(pathDir))
                {
                    Directory.CreateDirectory(pathDir);
                }

                var tagArr = NeuralNetwork.NeuralNetworkWorker.NeuralNetworkResultKTopPercent(image);
                if (TagValidator.CheckBadTag(tagArr))
                {
                    return;
                }
                if (tagArr[0] != currentTag)
                {
                    DataSetImage.Save(image, currentTag);
                }
            }
        }
    }
}
