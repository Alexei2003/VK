using DataSet;
using System.Net;

namespace DownloaderDataSetPhoto.Downloaders
{
    internal static class Downloader
    {
        public static void DownloadPhoto(WebClient wc, string url, string currentTag, string fileName)
        {
            Directory.CreateDirectory("DATA_SET");
            wc.DownloadFile(url, $"DATA_SET\\{fileName}.jpg");
            using var image = new Bitmap($"DATA_SET\\{fileName}.jpg");

            Directory.CreateDirectory("DATA_SET\\" + currentTag);


            if (NeuralNetwork.NeuralNetwork.NeuralNetworkResult(image) == currentTag)
            {
                return;
            }


            DataSetImage.Save(image, currentTag);
        }
    }
}
