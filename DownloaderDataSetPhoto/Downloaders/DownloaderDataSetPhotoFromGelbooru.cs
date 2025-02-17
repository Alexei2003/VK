using MyCustomClasses;
using System.Net;

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
                    using var wc = new WebClient();

                    var htmlDocument = Gelbooru.GetPageHTML(wc, url, i);

                    var nodesArr = htmlDocument.DocumentNode.SelectNodes("//img[contains(@src,'https') and img[contains(@src,'img3.gelbooru.com')]").ToArray();

                    foreach (var node in nodesArr)
                    {
                        var src = node.GetAttributeValue("src", string.Empty);
                        Downloader.DownloadPhoto(wc, src, currentTag, fileName + i.ToString());
                    }
                });
            }
            catch (Exception e)
            {
                Logs.WriteExcemption(e);
            }
        }
    }
}
