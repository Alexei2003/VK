using MyCustomClasses.VK;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace RepetitionOfPostsBot.BotsTask
{
    internal static class VKTask
    {
        private const string groupVKShortUrl = "@anime_art_for_every_day";
        private const Int64 groupVKId = 220199532;

        private static Random rand = new();

        public static void RepetitionOfVKPosts(object data)
        {
            var api = data as VkApiCustom;

            var wall = api.Wall.Get(new WallGetParams
            {
                OwnerId = -1 * groupVKId,
                Count = 1,
                Filter = WallFilter.Postponed
            });

            ulong indexResendedPost = Convert.ToUInt64(rand.Next(Convert.ToInt32(wall.TotalCount)));
            while (true)
            {
                try
                {
                    wall = api.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * groupVKId,
                        Count = 1,
                        Filter = WallFilter.Postponed
                    });

                    if (wall.WallPosts.Count < 1 || ((wall.WallPosts.First().Date.Value.Hour) != (DateTime.UtcNow.AddHours(1).Hour)))
                    {
                        wall = api.Wall.Get(new WallGetParams
                        {
                            OwnerId = -1 * groupVKId,
                            Count = 1,
                            Filter = WallFilter.All
                        });
                        var totalCountPosts = wall.TotalCount;
                        var offsetIndexPost = totalCountPosts - indexResendedPost;

                        if (offsetIndexPost < 1000)
                        {
                            indexResendedPost = 0;
                            continue;
                        }

                        var postData = wall.WallPosts[0].Date;

                        wall = api.Wall.Get(new WallGetParams
                        {
                            OwnerId = -1 * groupVKId,
                            Offset = offsetIndexPost,
                            Count = 1,
                            Filter = WallFilter.All
                        });

                        if (wall.WallPosts.Count == 0)
                        {
                            indexResendedPost++;
                            continue;
                        }

                        var post = wall.WallPosts.First();
                        var tags = post.Text;

                        if (tags.Split('#', StringSplitOptions.RemoveEmptyEntries).Length < 2 || tags.Contains('.') || tags.Contains(' '))
                        {
                            indexResendedPost++;
                            continue;
                        }

                        var tag = post.Text;

                        if (!tag.Contains('@'))
                        {
                            var tagsArr = tag.Split('#', StringSplitOptions.RemoveEmptyEntries);

                            tag = string.Join("", tagsArr.Select(s => "#" + s + groupVKShortUrl + "\n"));
                        }

                        api.Wall.Post(new WallPostParams()
                        {
                            OwnerId = -1 * groupVKId,
                            FromGroup = true,
                            Message = '.' + tag,
                            Attachments = new MediaAttachment[] { new PhotoMy { OwnerId = post.Attachment.Instance.OwnerId, Id = post.Attachment.Instance.Id, AccessKey = post.Attachment.Instance.AccessKey } },
                            PublishDate = postData.Value.AddHours(1),
                        });

                        indexResendedPost += Convert.ToUInt64(1 + rand.Next(Convert.ToInt32(totalCountPosts / 100)));
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(30));
                }
                catch
                {
                    indexResendedPost++;
                    continue;
                }
            }
        }
    }
}
