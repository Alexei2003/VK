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

                    var nodesArr = Gelbooru.GetObjectsNodes(htmlDocument, "//img[@src]", ["https", "img3.gelbooru.com"]);

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
