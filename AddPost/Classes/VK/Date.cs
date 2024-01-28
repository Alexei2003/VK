using MyCustomClasses;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace AddPost.Classes.VK
{
    internal sealed class Date
    {
        private readonly VkApiCustom api;

        public Date(VkApiCustom api)
        {
            this.api = api;
        }

        public DateTime? ChangeTimeNewPostUseLastPost(Int64 groupId, int hoursBetweenPost)
        {
            var data = GetTimeLastPost(groupId);

            data = data.Value.AddHours(hoursBetweenPost);

            return data;
        }

        private DateTime? GetTimeLastPost(Int64 groupId)
        {
            var post = api.Wall.Get(new WallGetParams()
            {
                OwnerId = -1 * groupId,
                Count = 100,
                Filter = WallFilter.Postponed,
            });

            if (post.WallPosts.Count < 1)
            {
                post = api.Wall.Get(new WallGetParams()
                {
                    OwnerId = -1 * groupId,
                    Count = 1,
                    Filter = WallFilter.All,
                });
            }
            else
            {
                if (post.TotalCount > 100)
                {
                    post = api.Wall.Get(new WallGetParams()
                    {
                        OwnerId = -1 * groupId,
                        Offset = post.TotalCount - 1,
                        Count = 1,
                        Filter = WallFilter.Postponed,
                    });
                }
            }

            return post.WallPosts.Last().Date;
        }
    }
}
