using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Text;
using System.Xml.Linq;
using VkNet;
using VkNet.Model;

namespace MyCustomClasses.VK.VKApiCustomClasses
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

        public ReadOnlyCollection<VkNet.Model.Photo> AddOnVKServer(Bitmap image, string name = "Post.jpg")
        {
            using var wc = new WebClient();

            image.Save(name, ImageFormat.Jpeg);

            return AddOnVKServer(wc, name);
        }

        public ReadOnlyCollection<VkNet.Model.Photo> AddOnVKServer(WebClient wc, string name)
        {
            var uploadServer = GetWallUploadServer();
            var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, name));

            return SaveWallPhoto(responseFile, null);
        }
    }
}
