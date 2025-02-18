using MyCustomClasses.Tags.Editors;
using MyCustomClasses.VK;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using VkNet.Model;

namespace AddPost.Classes
{
    public sealed class Post
    {
        private readonly VkApiCustom api;

        public Post(VkApiCustom api)
        {
            this.api = api;
        }

        public void Publish(Image<Rgb24>[] bmps, string tag, string copyright, DateTime? postDate, long groupId, string groupShortUrl)
        {
            var imageList = new List<VkNet.Model.Photo>(10);
            foreach (var bmp in bmps)
            {
                imageList.Add(api.Photo.AddOnVKServer(bmp)[0]);
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
