using System.Diagnostics.Metrics;

using NeuralNetwork;

using Other;

namespace DownloaderDataSetPhoto.Downloaders
{
    public static class DownloaderDataSetPhotoFromGelbooru
    {

        public static void SavePhotos(string url, string currentTag, string fileName, int countPages)
        {
            try
            {
                for (var i = 0; i < countPages; i++)
                {
                    using var httpClient = new HttpClient();

                    var htmlDocument = Gelbooru.GetPageHTML(httpClient, url, i);

                    var nodesArr = htmlDocument.DocumentNode.SelectNodes("//img[contains(@src,'https://gelbooru.com')]").ToArray();
                    Parallel.For(0, nodesArr.Length, NeuralNetworkWorker.ParallelOptions, j =>
                    {
                        var src = nodesArr[j].GetAttributeValue("src", string.Empty);
                        Downloader.DownloadPhoto(httpClient, new Uri(src), currentTag, fileName + j.ToString());
                    });
                }
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
            }
        }
    }
}
