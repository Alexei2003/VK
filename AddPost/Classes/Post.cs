using MyCustomClasses;
using VkNet.Model;

namespace AddPost
{
    internal class Post
    {
        private readonly VkApiCustom api;
        private readonly Classes.Photo photo;

        public Post(VkApiCustom api)
        {
            this.api = api;
            photo = new Classes.Photo(api);
        }

        public void Publish(Bitmap image, string tag, string copyright, DateTime? postDate, string groupId)
        {
            var postImage = photo.Add(image);
            api.Wall.Post(new WallPostParams()
            {
                OwnerId = -1 * Convert.ToInt64(groupId),
                FromGroup = true,
                Message = tag,
                Attachments = postImage,
                PublishDate = postDate,
                Copyright = copyright
            });
        }
    }
}
