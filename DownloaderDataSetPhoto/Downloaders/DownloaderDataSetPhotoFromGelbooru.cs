using MyCustomClasses;
using System.Net;

namespace DownloaderDataSetPhoto.Downloaders
{
    internal class DownloaderDataSetPhotoFromGelbooru
    {
        public static void SavePhotos(string url, string currentTag, float percentOriginalTag, string fileName)
        {
            url += "1girls+-completely_nude+-gangbang+-imminent_sex+-sex+-penis+-condom+-cum+-futa+-torn_clothing+-1boy+-rape+-overeating+-filled_condom+-big_belly+-bdsm+-gigantic_breasts" +
                   "+-hyper_breasts+-hyper_thighs+-hyper_ass+-chubby+-anal+-2girls+-cosplay+-text+-hypnosis+-slave+-pussy+-nose_hook+-2boys+-penis_over_eyes+-multiple_penises+-anal_insertion+-dildo" +
                   "+-object_insertion+-sex_toys+-bestiality+-licking_penis";

            try
            {

                Parallel.For(0, 10, i =>
                {
                    var fileNameBuff = fileName + i.ToString();

                    using var wc = new WebClient();
                    var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.CookieContainer = new CookieContainer();
                    using var httpClient = new HttpClient(httpClientHandler);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36");
                    
                    var tmpurl = url + "&pid=" + i * 42;
                    var html = httpClient.GetStringAsync(tmpurl);

                    var htmlDocument = new HtmlAgilityPack.HtmlDocument();
                    htmlDocument.LoadHtml(html.Result);

                    var imageNodes = htmlDocument.DocumentNode.SelectNodes("//img[@src]");
                    if (imageNodes != null)
                    {
                        foreach (var img in imageNodes)
                        {
                            if (!img.OuterHtml.Contains("https"))
                            {
                                continue;
                            }

                            if (!img.OuterHtml.Contains("img3.gelbooru.com"))
                            {
                                continue;
                            }

                            var src = img.GetAttributeValue("src", string.Empty);
                            SavePhoto(src, currentTag, percentOriginalTag, fileNameBuff, wc);
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Logs.WriteExcemption(e);
            }
        }

        private static void SavePhoto(string url, string currentTag, float percentOriginalTag, string fileName, WebClient wc)
        {
            Downloader.DownloadPhoto(wc, url, currentTag, percentOriginalTag, fileName);
        }
    }
}
