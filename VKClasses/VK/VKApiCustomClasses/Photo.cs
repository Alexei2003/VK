using System.Collections.ObjectModel;

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

        public ReadOnlyCollection<VkNet.Model.Photo> SaveWallPhoto(string response, ulong? userId, ulong? groupId = null, string? caption = null)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Photo.SaveWallPhoto(response, userId, groupId, caption);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public ReadOnlyCollection<VkNet.Model.Photo> AddOnVKServer(HttpClient httpClient, Image<Rgb24> image, string fileName = "Post.jpg")
        {
            image.SaveAsJpeg(fileName);

            return AddOnVKServer(httpClient, fileName);
        }

        public ReadOnlyCollection<VkNet.Model.Photo> AddOnVKServer(HttpClient httpClient, string fileName)
        {
            var uploadServer = GetWallUploadServer();
            var responseFile = ImageTransfer.UploadImageAsync(httpClient, new Uri(uploadServer.UploadUrl), fileName).Result;

            return SaveWallPhoto(responseFile, null);
        }
    }
}
