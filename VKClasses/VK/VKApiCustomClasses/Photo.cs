using System.Collections.ObjectModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using VkNet;
using VkNet.Model;

namespace VKClasses.VK.VKApiCustomClasses
{
    public class Photo
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Photo(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public UploadServerInfo GetWallUploadServer(long? groupId = null)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Photo.GetWallUploadServer(groupId);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        private class ResponseClass
        {
            public ReadOnlyCollection<VkNet.Model.Photo> Response { get; set; } = null;
        }

        public ReadOnlyCollection<VkNet.Model.Photo> SaveWallPhoto(string responseParam, ulong? userId, ulong? groupId = null, string? caption = null)
        {
            while (true)
            {
                try
                {
                    //return ApiOriginal.Photo.SaveWallPhoto(response, userId, groupId, caption);

                    var obj = JObject.Parse(responseParam);

                    // Получаем значения
                    var photo = (obj["photo"] ?? new object()).ToString();
                    var hash = (obj["hash"] ?? new object()).ToString();
                    var server = (obj["server"] ?? -1).ToString();

                    var response = ApiOriginal.Invoke("photos.saveWallPhoto",
                    new Dictionary<string, string>
                    {
                        { "v", "5.199" },
                        { "access_token", ApiOriginal.Token },
                        {"user_id", userId.ToString() ?? ""},
                        {"groupId", groupId.ToString() ?? "" },
                        {"photo", photo ?? "" },
                        {"server", server ?? "" },
                        {"hash", hash ?? "" },
                        {"caption", caption ?? "" },
                    });
                    response = response.Replace("base", "z");
                    var responseClass = JsonConvert.DeserializeObject<ResponseClass>(response);
                    return responseClass.Response;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public ReadOnlyCollection<VkNet.Model.Photo>? AddOnVKServer(HttpClient httpClient, Image<Rgb24> image, string fileName = "Post.jpg")
        {
            image.SaveAsJpeg(fileName);

            return AddOnVKServer(httpClient, fileName);
        }

        public ReadOnlyCollection<VkNet.Model.Photo>? AddOnVKServer(HttpClient httpClient, string fileName)
        {
            var uploadServer = GetWallUploadServer();
            var responseFile = ImageTransfer.UploadImageAsync(httpClient, new Uri(uploadServer.UploadUrl), fileName).Result;

            if (!string.IsNullOrEmpty(responseFile))
            {
                return SaveWallPhoto(responseFile, null);
            }
            else
            {
                return null;
            }
        }
    }
}
