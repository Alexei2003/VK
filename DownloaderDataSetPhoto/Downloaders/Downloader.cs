using DataSet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

            var filesList = Directory.GetFiles("DATA_SET\\" + currentTag);
            bool similar = false;
            foreach (var file in filesList)
            {
                var tmpImage = new Bitmap(file);
                if (DataSetPhoto.IsSimilarPhoto(DataSetPhoto.ChangeResolution224224(image), DataSetPhoto.ChangeResolution224224(tmpImage)))
                {
                    similar = true;
                }
            }
            if (!similar)
            {
                DataSetPhoto.Save(image, currentTag);
            }
        }
    }
}
