using MyCustomClasses;
using System.Net;

namespace DownloaderDataSetPhoto.Downloaders
{
    internal class DownloaderDataSetPhotoFromRule34
    {
        public static void SavePhotos(string url, string currentTag, float percentOriginalTag, string fileName, object lockNeuralNetworkResult)
        {
            using var wc = new WebClient();

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.CookieContainer = new CookieContainer();
            using var httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36");

            try
            {
                for (var i = 0; i < 10; i++)
                {
                    var tmpurl = url + "&pid=" + i * 42;
                    var html = httpClient.GetStringAsync(tmpurl);

                    var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                    htmlDocument.LoadHtml(html.Result);

                    var imageNodes = htmlDocument.DocumentNode.SelectNodes("//img[@src]");
                    if (imageNodes != null)
                    {
                        foreach (var img in imageNodes)
                        {
                            var src = img.GetAttributeValue("src", string.Empty);
                            var alt = img.GetAttributeValue("alt", string.Empty);
                            if (alt.Contains("futa") || alt.Length == 0 || alt.Split('(', StringSplitOptions.RemoveEmptyEntries).Length > 2)
                            {
                                continue;
                            }
                            SavePhoto(src, currentTag, percentOriginalTag, fileName, lockNeuralNetworkResult, wc);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logs.WriteExcemption(e);
            }
        }

        private static void SavePhoto(string url, string currentTag, float percentOriginalTag, string fileName, object lockNeuralNetworkResult, WebClient wc)
        {
            Downloader.DownloadPhoto(wc, url, currentTag, percentOriginalTag, fileName, lockNeuralNetworkResult);
        }
    }
}
