using MyCustomClasses;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Text;

namespace WorkWithPost
{
    public sealed class Photo
    {
        private readonly VkApiCustom api;
        public Photo(VkApiCustom api)
        {
            this.api = api;
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<VkNet.Model.Photo> AddOnVKServer(Bitmap image)
        {
            var uploadServer = api.Photo.GetWallUploadServer();
            using var wc = new WebClient();

            image.Save("Post.jpg", ImageFormat.Jpeg);

            var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, "Post.jpg"));

            return api.Photo.SaveWallPhoto(responseFile, Convert.ToUInt32(api.ApiOriginal.UserId));
        }
    }
}
