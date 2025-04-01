using System.Net;
using System.Text;

using Newtonsoft.Json.Linq;

using VkNet;
using VkNet.Model;

namespace VKClasses.VK.VKApiCustomClasses
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

        private void Save(string serverAnswer, string accessToken)
        {
            while (true)
            {
                try
                {
                    ApiOriginal.Invoke("stories.save",
                    new Dictionary<string, string>
                    {
                        {"v","5.81"},
                        {"access_token", accessToken},
                        {"upload_results", serverAnswer}
                    });
                    return;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public static void Post(GetPhotoUploadServerParams @param, string accessToken, string path)
        {
            var api = new VkApiCustom(accessToken);

            var response = api.ApiOriginal.Stories.GetPhotoUploadServer(@param);

            using var wc = new WebClient();
            var responseFile = Encoding.ASCII.GetString(wc.UploadFile(response.UploadUrl, path));

            var json = JObject.Parse(responseFile);
            string uploadResult = json["response"]["upload_result"].ToString();

            api.Stories.Save(uploadResult, accessToken);
        }
    }
}
