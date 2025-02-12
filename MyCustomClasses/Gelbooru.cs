using HtmlAgilityPack;
using System.Net;

namespace MyCustomClasses
{
    public static class Gelbooru
    {
        private const string NO_SEARCH = "1girls+-completely_nude+-gangbang+-imminent_sex+-sex+-penis+-condom+-cum+-futa+-torn_clothing+-1boy+-rape+-overeating+-filled_condom+-big_belly+-bdsm+-gigantic_breasts+-hyper_breasts+-hyper_thighs+-hyper_ass+-chubby+-anal+-2girls+-cosplay+-text+-hypnosis+-slave+-pussy+-nose_hook+-2boys+-penis_over_eyes+-multiple_penises+-anal_insertion+-dildo+-object_insertion+-sex_toys+-bestiality+-licking_penis+-areolae+-backboob+-animated+-foot_focus+-english_text+-horror_(theme)+-multiple_boys+-3boys+-topless+-hatching_(texture)+-crotch_focus+-head_out_of_frame+-foot_worship+-no_bra+-no_panties+-censored+-peeing+-masturbation+-ass_focus+-nude+-feet_only+-lower_body";

        public static HtmlDocument GetPageHTML(WebClient wc, string url, int indexPage = -1, bool addNoSearch = true)
        {
            if (addNoSearch)
            {
                url += url[^1] == '+' ? NO_SEARCH : '+' + NO_SEARCH;
            }

            if (indexPage > -1)
            {
                url += "&pid=" + indexPage * 42;
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

        public static HtmlNode[] GetObjectsNodes(HtmlDocument htmlDocument, string selectNode, string[] uncorrectParts)
        {
            var result = new List<HtmlNode>();

            var nodes = htmlDocument.DocumentNode.SelectNodes(selectNode);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (CorrectNode(node, uncorrectParts))
                    {
                        result.Add(node);
                    }
                }
            }

            return result.ToArray();
        }

        private static bool CorrectNode(HtmlNode node, string[] uncorrectParts)
        {
            foreach (var part in uncorrectParts)
            {
                if (!node.OuterHtml.Contains(part))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
