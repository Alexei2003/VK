using MyCustomClasses;
using System.Drawing.Imaging;
using System.Net;
using System.Text;

namespace AddPost.Classes
{
    internal class Photo
    {
        private readonly VkApiCustom api;
        public Photo(VkApiCustom api)
        {
            this.api = api;
        }

        public IReadOnlyCollection<VkNet.Model.Photo> Add(Bitmap image)
        {
            var uploadServer = api.Photo.GetWallUploadServer();
            using (var wc = new WebClient())
            {
                byte[] imageBytes;
                using (var ms = new MemoryStream())
                {
                    image.Save("1.jpg", ImageFormat.Jpeg);
                    imageBytes = ms.ToArray();
                }
                var responseFile = Encoding.ASCII.GetString(wc.UploadFile(uploadServer.UploadUrl, "1.jpg"));

                return api.Photo.SaveWallPhoto(responseFile, Convert.ToUInt32(api.ApiOriginal.UserId));
            }
        }
    }
}
