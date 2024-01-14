using MyCustomClasses;
using VkNet.Model;

namespace AddPost.Classes.VK
{
    internal class Post
    {
        private readonly VkApiCustom api;
        private readonly Photo photo;

        public Post(VkApiCustom api)
        {
            this.api = api;
            photo = new Classes.VK.Photo(api);
        }

        public void Publish(Bitmap image, string tag, string copyright, DateTime? postDate, Int64 groupId)
        {
            var postImage = photo.AddOnVKServer(image);
            api.Wall.Post(new WallPostParams()
            {
                OwnerId = -1 * groupId,
                FromGroup = true,
                Message = tag,
                Attachments = postImage,
                PublishDate = postDate,
                Copyright = copyright
            });
        }
    }
}
