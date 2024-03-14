using VkNet.Model;

namespace AddPost.Classes
{
    public sealed class Post
    {
        private readonly MyCustomClasses.VkApiCustom api;
        private readonly Photo photo;

        public Post(MyCustomClasses.VkApiCustom api)
        {
            this.api = api;
            photo = new Photo(api);
        }

        public void Publish(Bitmap[] images, string tag, string copyright, DateTime? postDate, long groupId)
        {
            var imageList = new List<VkNet.Model.Photo>(10);
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
