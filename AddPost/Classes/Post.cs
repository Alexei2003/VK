using System.Net;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using VKClasses.Tags.Editors;
using VKClasses.VK;

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

        public void Publish(Image<Rgb24>[] images, string tag, string copyright, DateTime? postDate, long groupId, string groupShortUrl)
        {
            var imageList = new List<Photo>(10);
            var wc = new WebClient();
            foreach (var image in images)
            {
                imageList.Add(api.Photo.AddOnVKServer(wc, image)[0]);
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
