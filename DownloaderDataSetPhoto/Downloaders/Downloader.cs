using DataSet;
using SixLabors.ImageSharp.PixelFormats;
using System.Net;

namespace DownloaderDataSetPhoto.Downloaders
{
    internal static class Downloader
    {
        public static void DownloadPhoto(WebClient wc, string url, string currentTag, string fileName)
        {
            Directory.CreateDirectory("DATA_SET");
            wc.DownloadFile(url, $"DATA_SET\\{fileName}.jpg");
            using var bmp = SixLabors.ImageSharp.Image.Load<Rgb24>($"DATA_SET\\{fileName}.jpg");

            var pathDir = "DATA_SET\\" + currentTag;
            if (!Directory.Exists(pathDir))
            {
                Directory.CreateDirectory(pathDir);
            }

            if (NeuralNetwork.NeuralNetwork.NeuralNetworkResult(bmp) == currentTag)
            {
                return;
            }


            DataSetImage.Save(bmp, currentTag);
        }
    }
}
