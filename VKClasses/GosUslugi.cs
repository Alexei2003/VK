using System.Text.Json;

namespace VKClasses
{
    public static class GosUslugi
    {
        public const string VK = "VK";
        public const string FACEBOOK = "FACEBOOK";
        public const string TELEGRAM = "TELEGRAM";
        public const string TIKTOK = "TIKTOK";
        public const string DISCORD = "DISCORD";

        private const string _path = "E:\\WPS\\CommonData\\VK\\MyAccessTokens.txt";
        public static Dictionary<string, string> GetAccessTokens()
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        }
    }
}
