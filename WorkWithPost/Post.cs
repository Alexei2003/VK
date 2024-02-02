using MyCustomClasses;
using System.Drawing;
using VkNet.Model;

namespace WorkWithPost
{
    public sealed class Post
    {
        private readonly VkApiCustom api;
        private readonly Photo photo;

        public Post(VkApiCustom api)
        {
            this.api = api;
            photo = new Photo(api);
        }

        public void Publish(Bitmap[] images, string tag, string copyright, DateTime? postDate, Int64 groupId)
        {
            var imageList = new List<VkNet.Model.Photo>();
            foreach (var image in images)
            {
                imageList.Add(photo.AddOnVKServer(image).First());
            }
            api.Wall.Post(new WallPostParams()
            {
                OwnerId = -1 * groupId,
                FromGroup = true,
                Message = tag,
                Attachments = imageList,
                PublishDate = postDate,
                Copyright = copyright
            });
        }
    }
}
