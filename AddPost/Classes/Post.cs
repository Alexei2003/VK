using MyCustomClasses.Tags.Editors;
using MyCustomClasses.VK;
using VkNet.Model;

namespace AddPost.Classes
{
    public sealed class Post
    {
        private readonly VkApiCustom api;
        private readonly Image photo;

        public Post(VkApiCustom api)
        {
            this.api = api;
            photo = new Image(api);
        }

        public void Publish(Bitmap[] images, string tag, string copyright, DateTime? postDate, long groupId, string groupShortUrl)
        {
            var imageList = new List<VkNet.Model.Photo>(10);
            foreach (var image in images)
            {
                imageList.Add(photo.AddOnVKServer(image).First());
            }

            var tags = tag.Split('#', StringSplitOptions.RemoveEmptyEntries);

            tag = string.Join("", tags.Select(s => "#" + s + groupShortUrl + "\n"));

            tag = BaseTagsEditor.GetBaseTagsWithNextLine() + tag;

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
