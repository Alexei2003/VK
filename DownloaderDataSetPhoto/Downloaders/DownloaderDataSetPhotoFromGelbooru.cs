using Other;

using VKClasses;

namespace DownloaderDataSetPhoto.Downloaders
{
    internal static class DownloaderDataSetPhotoFromGelbooru
    {

        public static void SavePhotos(string url, string currentTag, string fileName)
        {
            try
            {

                Parallel.For(0, 10, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, i =>
                {
                    using var httpClient = new HttpClient();

                    var htmlDocument = Gelbooru.GetPageHTML(httpClient, url, i);

                    var nodesArr = htmlDocument.DocumentNode.SelectNodes("//img[contains(@src,'https') and contains(@src,'img4.gelbooru.com')]").ToArray();
                    foreach (var node in nodesArr)
                    {
                        var src = node.GetAttributeValue("src", string.Empty);
                        Downloader.DownloadPhoto(httpClient, new Uri(src), currentTag, fileName + i.ToString());
                    }
                });
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
            }
        }
    }
}
