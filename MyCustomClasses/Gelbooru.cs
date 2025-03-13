using HtmlAgilityPack;
using System.Net;

namespace MyCustomClasses
{
    public static class Gelbooru
    {
        private const string NO_SEARCH =
            // Сексуальный контент и анатомия
            "+-nipples+-completely_nude+-gangbang+-imminent_sex+-sex+-penis+-condom+-cum+-futa+-rape" +
            "+-filled_condom+-anal+-pussy+-multiple_penises+-anal_insertion+-dildo+-object_insertion" +
            "+-sex_toys+-bestiality+-licking_penis+-areolae+-backboob+-masturbation+-peeing+-nude" +

            // Гипертрофированные/преувеличенные части тела
            "+-gigantic_breasts+-hyper_breasts+-hyper_thighs+-hyper_ass+-chubby+-big_belly+-overeating" +

            // Фетиш и BDSM
            "+-bdsm+-hypnosis+-slave+-nose_hook+-penis_over_eyes+-foot_worship" +

            // Текст, графика и тематика
            "+-cosplay+-text+-english_text+-horror_(theme)+-hatching_(texture)+-animated" +

            // Фокус на определенных частях тела
            "+-lower_body+-crotch_focus+-head_out_of_frame+-ass_focus+-feet_only+-foot_focus" +

            // Одежда и нижнее белье
            "+-torn_clothing+-topless+-no_bra+-no_panties+-censored" +

            // Количество девушек
            "+1girl+-2girls+-3girls+-4girls+-5girls+-6%2Bgirls+-multiple_girls" +

            // Количество мальчиков
            "+-1boy+-2boys+-3boys+-4boys+-5boys+-6%2Bboys+-multiple_boys";


        public static HtmlDocument GetPageHTML(WebClient wc, string url, int indexPage = -1, bool useProxy = false)
        {
            if (indexPage > -1)
            {
                url += url[^1] == '+' ? NO_SEARCH : '+' + NO_SEARCH;
                url += "&pid=" + indexPage * 42;

                if (useProxy)
                {
                    url += $"&temp={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{new Random().Next(0, 9999)}";
                    url = GetUrlUseMirror(url);
                }
            }

            if (url[0] != 'h')
            {
                url = GetUrlAddMirrorServer(url);
            }

            var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = new CookieContainer()
            };
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
