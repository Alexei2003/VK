using HtmlAgilityPack;
using System.Net;

namespace MyCustomClasses
{
    public static class Gelbooru
    {
        private const string NO_SEARCH = "1girls+-nipples+-completely_nude+-gangbang+-imminent_sex+-sex+-penis+-condom+-cum+-futa+-torn_clothing+-1boy+-rape+-overeating+-filled_condom+-big_belly+-bdsm+-gigantic_breasts+-hyper_breasts+-hyper_thighs+-hyper_ass+-chubby+-anal+-2girls+-cosplay+-text+-hypnosis+-slave+-pussy+-nose_hook+-2boys+-penis_over_eyes+-multiple_penises+-anal_insertion+-dildo+-object_insertion+-sex_toys+-bestiality+-licking_penis+-areolae+-backboob+-animated+-foot_focus+-english_text+-horror_(theme)+-multiple_boys+-3boys+-topless+-hatching_(texture)+-crotch_focus+-head_out_of_frame+-foot_worship+-no_bra+-no_panties+-censored+-peeing+-masturbation+-ass_focus+-nude+-feet_only+-lower_body";

        public static HtmlDocument GetPageHTML(WebClient wc, string url, int indexPage = -1, bool useProxy = false)
        {
            if (indexPage > -1)
            {
                url += url[^1] == '+' ? NO_SEARCH : '+' + NO_SEARCH;
                url += "&pid=" + indexPage * 42;

                if (useProxy)
                {
                    url += $"&t={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{new Random().Next(0, 9999)}";
                    url = GetUrlUseMirror(url);
                }
            }

            if (url[0] != 'h')
            {
                url = GetUrlAddMirrorServer(url);
            }

            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.CookieContainer = new CookieContainer();
            using var httpClient = new HttpClient(httpClientHandler);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/97.0.4692.71 Safari/537.36");

            var html = httpClient.GetStringAsync(url);
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html.Result);

            return htmlDocument;
        }


        public static string GetUrlUseMirror(string url)
        {
            // Преобразуем URL в percent-encoded
            url = Uri.EscapeDataString(url);

            const string MIRROR_URL = "https://siteget.net/o.php?b=4&f=norefer&_senable_sig=&pv=0&mobile=&u=";

            return MIRROR_URL + url;
        }

        public static string GetUrlAddMirrorServer(string url)
        {
            const string MIRROR_URL = "https://siteget.net";

            return MIRROR_URL + url;
        }
    }
}
