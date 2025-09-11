using Other;

namespace DownloaderDataSetPhoto.Downloaders
{
    public static class DownloaderDataSetPhotoFromGelbooru
    {

        public static void SavePhotos(string url, string currentTag, string fileName)
        {
            try
            {

                for (var i = 0; i < 10; i++)
                {
                    using var httpClient = new HttpClient();

                    var htmlDocument = Gelbooru.GetPageHTML(httpClient, url, i);

                    var nodesArr = htmlDocument.DocumentNode.SelectNodes("//img[contains(@src,'https://gelbooru.com')]").ToArray();
                    foreach (var node in nodesArr)
                    {
                        var src = node.GetAttributeValue("src", string.Empty);
                        Downloader.DownloadPhoto(httpClient, new Uri(src), currentTag, fileName + i.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
            }
        }
    }
}
