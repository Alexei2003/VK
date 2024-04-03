using MyCustomClasses;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace RepetitionOfPostsBot
{
    internal static class Program
    {
        private const Int64 groupId = 220199532;
        private static Random rand = new();
        private static void Main()
        {
            var accessToken = File.ReadAllText("AccessToken.txt");
            var api = new VkApiCustom(accessToken);

            var threadRepetitionOfPosts = new Thread(new ParameterizedThreadStart(RepetitionOfPosts));
            threadRepetitionOfPosts.Start(api);

            while (true)
            {
                Thread.Sleep(TimeSpan.FromDays(1));
            }
        }

        public static void RepetitionOfPosts(object data)
        {
            var api = data as MyCustomClasses.VkApiCustom;

            ulong indexResendedPost = 0;
            while (true)
            {
                try
                {
                    var wall = api.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * groupId,
                        Count = 1,
                        Filter = WallFilter.Postponed
                    });

                    if (wall.WallPosts.Count < 1 || ((wall.WallPosts.First().Date.Value.Hour) != (DateTime.UtcNow.AddHours(1).Hour)))
                    {
                        wall = api.Wall.Get(new WallGetParams
                        {
                            OwnerId = -1 * groupId,
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
                            OwnerId = -1 * groupId,
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

                        if (tags.Split('#', StringSplitOptions.RemoveEmptyEntries).Length < 2 || tags.Contains('.') || tags.Contains(' ') || tags.Contains('\n'))
                        {
                            indexResendedPost++;
                            continue;
                        }

                        api.Wall.Post(new WallPostParams()
                        {
                            OwnerId = -1 * groupId,
                            FromGroup = true,
                            Message = '.' + post.Text,
                            Attachments = new MediaAttachment[] { new PhotoMy { OwnerId = post.Attachment.Instance.OwnerId, Id = post.Attachment.Instance.Id, AccessKey = post.Attachment.Instance.AccessKey } },
                            PublishDate = postData.Value.AddHours(1),
                        });
                        indexResendedPost += Convert.ToUInt64(1 + rand.Next(10));
                    }
                    else
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(30));
                    }
                }
                catch
                {
                    indexResendedPost++;
                    continue;
                }
            }
        }

        public static void ShowPopularPosts(object data)
        {
            while (true)
            {
                while (DateTime.UtcNow.Minute < 5 || DateTime.UtcNow.Minute > 10) ;

            }
        }
    }
}