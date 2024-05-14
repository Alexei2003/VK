using Newtonsoft.Json;

namespace MyCustomClasses
{
    public static class GosUslugi
    {
        public const string VK = "VK";
        public const string INSTAGRAM = "Instagram";

        public static Dictionary<string, string> GetAccessTokens()
        {
            var json = File.ReadAllText("MyAccessTokens.txt");
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
}
