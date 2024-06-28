using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using VkNet;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Utils;

namespace MyCustomClasses.VK.VKApiCustomClasses
{
    public class Stories
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Stories(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        private StoryServerUrl GetPhotoUploadServer(GetPhotoUploadServerParams @param)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Stories.GetPhotoUploadServer(@param);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        private string Save(string serverAnswer, string accessToken)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Invoke("stories.save",
                    new Dictionary<string, string>
                    {
                        {"access_token", accessToken},
                        {"upload_results", serverAnswer},
                        {"v","5.81"}
                    });
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public void Post(GetPhotoUploadServerParams @param, string accessToken, string path)
        {
            var api = new VkApiCustom(accessToken);

            var response = api.ApiOriginal.Stories.GetPhotoUploadServer(@param);

            using var wc = new WebClient();
            var responseFile = Encoding.ASCII.GetString(wc.UploadFile(response.UploadUrl, path));

            var json = JObject.Parse(responseFile);
            string uploadResult = json["response"]?["upload_result"]?.ToString();

            var story = api.Stories.Save(uploadResult, accessToken);
        }
    }
}
