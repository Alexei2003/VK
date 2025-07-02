using VKClasses.VK;

using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace AddPost.Classes
{
    public sealed class Date
    {
        private readonly VkApiCustom api;

        public Date(VkApiCustom api)
        {
            this.api = api;
        }

        public DateTime? ChangeTimeNewPostUseLastPost(long groupId, int hoursBetweenPost)
        {
            var data = GetTimeLastPost(groupId);

            data = data.Value.AddHours(hoursBetweenPost);

            return data;
        }

        private DateTime? GetTimeLastPost(long groupId)
        {
            var wall = api.Wall.Get(new WallGetParams()
            {
                OwnerId = -1 * groupId,
                Count = 100,
                Filter = WallFilter.Postponed,
            });

            VkNet.Model.Post post;

            if (wall.WallPosts.Count < 1)
            {
                wall = api.Wall.Get(new WallGetParams()
                {
                    OwnerId = -1 * groupId,
                    Count = 2,
                    Filter = WallFilter.All,
                });

                if (wall.WallPosts[0].IsPinned != null)
                {
                    post = wall.WallPosts[1];
                }
                else
                {
                    post = wall.WallPosts[0];
                }
            }
            else
            {
                if (wall.TotalCount > 100)
                {
                    wall = api.Wall.Get(new WallGetParams()
                    {
                        OwnerId = -1 * groupId,
                        Offset = wall.TotalCount - 1,
                        Count = 1,
                        Filter = WallFilter.Postponed,
                    });
                }
                post = wall.WallPosts[^1];
            }

            return post.Date;
        }
    }
}
