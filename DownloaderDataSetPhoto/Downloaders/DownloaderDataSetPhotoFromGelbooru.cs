using Other;

namespace DownloaderDataSetPhoto.Downloaders
{
    public static class DownloaderDataSetPhotoFromGelbooru
    {
        private static HttpClient _httpClient = new HttpClient();

        static DownloaderDataSetPhotoFromGelbooru()
        {
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Referrer = new Uri("https://gelbooru.com/");
        }

        public static List<string> CreateDownloadList(LastViewedDictionary lastViewedDictionary, string url, string currentTag, string fileName, int countPages)
        {
            var oldUrl = "";
            var srcList = new List<string>();
            for (var i = 0; i < countPages; i++)
            {
                var htmlDocument = Gelbooru.GetPageHTML(_httpClient, url, i);
                var nodesArr = htmlDocument.DocumentNode.SelectNodes("//img[contains(@src,'https://gelbooru.com')]").ToArray();

                if (i == 0)
                {
                    var newUrl = nodesArr[1].GetAttributeValue("src", string.Empty);
                    oldUrl = lastViewedDictionary.SwapUrl(currentTag, newUrl);
                }

                foreach (var node in nodesArr)
                {
                    var src = node.GetAttributeValue("src", string.Empty);
                    if (oldUrl != src)
                    {
                        srcList.Add(src);
                    }
                    else
                    {
                        return srcList;
                    }
                }
            }

            return srcList;
        }

        public static void SavePhotos(LastViewedDictionary lastViewedDictionary, string url, string currentTag, string fileName, int countPages)
        {
            try
            {
                var srcList = CreateDownloadList(lastViewedDictionary, url, currentTag, fileName, countPages);

                Parallel.For(0, srcList.Count, j =>
                {
                    Downloader.DownloadPhoto(_httpClient, new Uri(srcList[j]), currentTag, fileName + j.ToString());
                });
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
            }
        }
    }
}
