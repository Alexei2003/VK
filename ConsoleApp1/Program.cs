using Newtonsoft.Json.Linq;

using VKClasses;
using VKClasses.VK;

internal class Program
{
    private static void Main(string[] args)
    {
        var token = "";
        var path = "D:\\0.mp4";


        var test = new VkApiCustom(token);

        var str = test.ApiOriginal.Invoke("shortVideo.create",
        new Dictionary<string, string>
        {
            {"v","5.199"},
            {"access_token", token},
            {"group_id", "220199532"},
            {"wallpost", "0"},
            {"description", "test" },
            {"file_size", "1000000" }
        });
        var json = JObject.Parse(str);

        var url = json["response"]["upload_url"].ToString();
        using var httpClient = new HttpClient();
        var responseFile = ImageTransfer.UploadImageAsync(httpClient, new Uri(url), path).Result;

        json = JObject.Parse(responseFile);

        var videoId = json["response"]["video_id"].ToString();
        var videoHash = json["response"]["video_hash"].ToString();
        str = test.ApiOriginal.Invoke("shortVideo.publish",
        new Dictionary<string, string>
        {
                    {"v","5.199"},
                    {"access_token", token},
                    {"owner_id", "-220199532"},
                    {"video_id", videoId},
                    {"license_agreement",  "true"}
        });
    }
}