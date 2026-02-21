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
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                    httpClient.DefaultRequestHeaders.Referrer = new Uri("https://gelbooru.com/");

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
