using Newtonsoft.Json;

namespace MyCustomClasses
{
    public static class GosUslugi
    {
        public const string VK = "VK";
        public const string FACEBOOK = "FACEBOOK";
        public const string TELEGRAM = "TELEGRAM";
        public const string TIKTOK = "TIKTOK";
        public const string DISCORD = "DISCORD";

        public static Dictionary<string, string> GetAccessTokens()
        {
            var json = File.ReadAllText("MyAccessTokens.txt");
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
}
