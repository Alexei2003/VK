using HtmlAgilityPack;

namespace Other
{
    public static class Gelbooru
    {
        public static bool UseProxy { get; set; } = false;

        public const string NoSearch =
            // Сексуальный контент и анатомия
            "-nipples+-completely_nude+-gangbang+-imminent_sex+-sex+-penis+-condom+-cum+-futa+-rape" +
            "+-filled_condom+-anal+-pussy+-multiple_penises+-anal_insertion+-dildo+-object_insertion" +
            "+-sex_toys+-licking_penis+-areolae+-backboob+-masturbation+-peeing+-nude+-group_sex+-vaginal" +
            "+-handjob+-fellatio+-oral+-erection+-bdsm+-hypnosis+-slave+-nose_hook+-penis_over_eyes+-foot_worship" +
            "+-vibrator+-vibrator_cord+-vibrator_in_thigh_strap+-mind_control+-object_insertion+-pussy_juice" +
            "+-squatting+-baby+-breasts_out+-inverted_nipples+-topless_female+-spread_pussy+-spreading_own_pussy" +

            // Гипертрофированные/преувеличенные части тела
            "+-gigantic_breasts+-hyper_breasts+-hyper_thighs+-hyper_ass+-chubby+-big_belly+-overeating+-pregnant+-fat" +

            // Текст, графика и тематика
            "+-cosplay+-text+-english_text+-horror_(theme)+-hatching_(texture)+-animated" +

            // Фокус на определенных частях тела
            "+-lower_body+-crotch_focus+-head_out_of_frame+-ass_focus+-feet_only+-foot_focus+-ass" +

            // Одежда и нижнее белье
            "+-torn_clothing+-topless+-no_bra+-no_panties+-censored" +

            // Количество девушек
            "+1girl+-2girls+-3girls+-4girls+-5girls+-multiple_girls" +

            // Количество мальчиков
            "+-1boy+-2boys+-3boys+-4boys+-5boys+-multiple_boys" +

            // Существа
            "+-bestiality+-tentacles";

        public static HtmlDocument GetPageHTML(HttpClient httpClient, string url, int indexPage = -1)
        {
            if (indexPage > -1)
            {
                url += url[^1] == '+' ? NoSearch : '+' + NoSearch;
                url += "&pid=" + indexPage * 42;

                if (UseProxy)
                {
                    url += $"&temp={DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{RandomStatic.Rand.Next(0, 9999)}";
                    url = GetUrlUseMirror(url);
                }
            }

            if (url[0] != 'h')
            {
                url = GetUrlAddMirrorServer(url);
            }

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
            if (UseProxy)
            {
                const string MIRROR_URL = "https://siteget.net";

                return MIRROR_URL + url;
            }
            return url;
        }
    }
}
