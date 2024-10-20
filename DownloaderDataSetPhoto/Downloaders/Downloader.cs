using DataSet;
using System.Net;

namespace DownloaderDataSetPhoto.Downloaders
{
    internal class Downloader
    {
        public static void DownloadPhoto(WebClient wc, string url, string currentTag, float percentOriginalTag, string fileName, object lockNeuralNetworkResult)
        {
            Directory.CreateDirectory("DATA_SET");
            wc.DownloadFile(url, $"DATA_SET\\{fileName}.jpg");
            using var image = new Bitmap($"DATA_SET\\{fileName}.jpg");

            Directory.CreateDirectory("DATA_SET\\" + currentTag);

            lock (lockNeuralNetworkResult)
            {
                if (NeuralNetwork.NeuralNetwork.NeuralNetworkResult(image, percentOriginalTag) == currentTag)
                {
                    return;
                }
            }

            DataSetPhoto.Save(image, currentTag);
        }
    }
}
