using VkNet.Enums.StringEnums;
using VkNet.Model;
using WorkWithPost;

internal static class Program
{
    private const Int64 groupId = 220199532;
    private static void Main()
    {
        var accessToken = File.ReadAllText("AccessToken.txt");
        var authorize = new Authorize(accessToken);
        var api = authorize.Api;

        ulong indexResendedPost = 10;

        while (true)
        {
            Console.WriteLine($"Index post = {indexResendedPost}");
            try
            {
                var wall = api.Wall.Get(new WallGetParams
                {
                    OwnerId = -1 * groupId,
                    Count = 1,
                    Filter = WallFilter.Postponed
                });

                if (wall.WallPosts.Count > 0 && ((wall.WallPosts.First().Date.Value.AddHours(3).Hour) != (DateTime.Now.AddHours(1).Hour)))
                {
                    wall = api.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * groupId,
                        Count = 1,
                        Filter = WallFilter.All
                    });
                    var totalCountPosts = wall.TotalCount;
                    var offsetIndexPost = totalCountPosts - indexResendedPost;

                    if(offsetIndexPost < 1000)
                    {
                        indexResendedPost = 1;
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

                    var post = wall.WallPosts.First();
                    var tags = post.Text;

                    if (tags.Split('#').Length < 2 || tags.Contains('.') || tags.Contains(' ') || tags.Contains('\n'))
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
                }
                else
                {
                    Thread.Sleep(TimeSpan.FromHours(1));
                }
            }
            catch 
            {
                continue;
            }
        }
    }

    [Serializable]
    public sealed class PhotoMy : MediaAttachment, IGroupUpdate
    {
        /// <inheritdoc />
        protected override string Alias => "photo";
    }
}