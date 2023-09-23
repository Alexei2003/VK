using MyCustomClasses;
using MyCustomClasses.VkApiCustomClasses;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using VkNet.Enums.StringEnums;
using VkNet.Model;
using VkNet.Utils;

namespace LikesRepostsBots.Classes
{
    internal class SpamBot
    {
        private readonly VkApiCustom api = new();
        private readonly Random rand;
        private const int CHANCE_LIKE = 5;
        private const int MAX_COUNT_POST = 8;
        private readonly PeopleDictionary people;

        public SpamBot(string accessToken, PeopleDictionary people, Random rand)
        {
            api.Authorize(new ApiAuthParams
            {
                AccessToken = accessToken
            });
            api.Account.SetOnline(false);
            this.people = people;
            this.rand = rand;
        }

        private void WorkWithPosts(string groupId)
        {
            var wall = api.Wall.Get(new WallGetParams
            {
                OwnerId = -1 * Convert.ToInt64(groupId),
                Count = MAX_COUNT_POST,
                Filter = WallFilter.All
            });

            var botWall = api.Wall.Get(new WallGetParams
            {
                OwnerId = api.ApiOriginal.UserId,
                Count = 1,
                Filter = WallFilter.All
            });

            int countPosts;
            if (botWall.WallPosts[0].CopyHistory != null)
            {

                for (countPosts = 0; countPosts < MAX_COUNT_POST; countPosts++)
                {
                    if (wall.WallPosts[countPosts].Id == botWall.WallPosts[0].CopyHistory[0].Id)
                    {
                        break;
                    }
                }
            }
            else
            {
                countPosts = MAX_COUNT_POST;
            }

            RepostResult repostResult;
            bool likeOriginal = true;
            for (int numbPost = countPosts - 1; numbPost > -1;)
            {
                if (rand.Next((countPosts - numbPost) / 2 + 1) == 0)
                {
                    repostResult = api.Wall.Repost("wall" + wall.WallPosts[numbPost].OwnerId + "_" + wall.WallPosts[numbPost].Id, "", api.ApiOriginal.UserId, false);
                    Console.WriteLine("репост");

                    api.Likes.Add(new LikesAddParams
                    {
                        Type = LikeObjectType.Post,
                        ItemId = Convert.ToInt64(repostResult.PostId),
                    });
                    likeOriginal = false;
                }

                int likes = AddCommentsLike(groupId, wall.WallPosts[numbPost].Id);
                if (likes > 0)
                {
                    Console.WriteLine($"Число лайкнутых комментариев {likes}");
                }

                if (likeOriginal)
                {
                    api.Likes.Add(new LikesAddParams
                    {
                        Type = LikeObjectType.Post,
                        OwnerId = wall.WallPosts[numbPost].OwnerId,
                        ItemId = Convert.ToInt64(wall.WallPosts[numbPost].Id),
                    });
                }
                else
                {
                    likeOriginal = true;
                }

                Console.WriteLine($"{countPosts - numbPost}/{countPosts}");
                numbPost--;
            }
        }

        private void WorkWithFriends()
        {

            AddToFriendsFromRecomendedList();

        }

        private void AddToFriendsFromRecomendedList()
        {
            Console.WriteLine("Добавление друзей");
            ulong countFriends = 1;

            var suggestions = api.Friends.GetSuggestions();

            int index = 0;
            int banCount = 0;
            int newUserCount = 0;
            for (ulong i = 0; i < countFriends && index < suggestions.Count; index++)
            {
                if (!people.Contains(suggestions[index].Id))
                {
                    if (!IsMassAccount(suggestions[index].Id))
                    {
                        api.Friends.Add(suggestions[index].Id);
                        i++;
                    }
                    else
                    {
                        api.Account.Ban(suggestions[index].Id);
                        banCount++;
                    }
                    people.Add(suggestions[index].Id);
                    newUserCount++;
                }
                else
                {
                    api.Account.Ban(suggestions[index].Id);
                    banCount++;
                }
                if (index % 2 == 0)
                {
                    Console.Write("/");
                }
                else
                {
                    Console.Write("\\");
                }
            }
            Console.WriteLine();
            Console.WriteLine($"Количество забанненых аккаунтов {banCount}");
            Console.WriteLine($"Количество новых аккаунтов в списке  {newUserCount}");
        }

        private bool IsMassAccount(long personId)
        {
            const int COUNT_FRIENDS = 500;
            const int COUNT_FOLLOWING = 500;
            const int COUNT_FOLOWERS = 500;

            var user = api.Users.Get(new long[] { personId });
            if (user[0].IsClosed == true)
            {
                return true;
            }

            var friends = api.Friends.Get(new FriendsGetParams
            {
                UserId = personId,
            });
            if (friends != null && friends.TotalCount > COUNT_FRIENDS)
            {
                return true;
            }

            var following = api.Users.GetSubscriptions(personId);
            if (following != null && following.TotalCount > COUNT_FOLLOWING)
            {
                return true;
            }

            var followers = api.Users.GetFollowers(personId);
            if (followers != null && followers.TotalCount > COUNT_FOLOWERS)
            {
                return true;
            }

            return false;
        }

        private int AddCommentsLike(string groupId, long? postId)
        {
            var comments = api.Wall.GetComments(new WallGetCommentsParams
            {
                OwnerId = -1 * Convert.ToInt64(groupId),
                PostId = Convert.ToInt64(postId),
                Count = 100,

            });

            int likes = 0;
            foreach (var comment in comments.Items)
            {
                int randNumb = rand.Next(CHANCE_LIKE);
                if (randNumb == 0)
                {
                    api.Likes.Add(new LikesAddParams
                    {
                        Type = LikeObjectType.Comment,
                        ItemId = Convert.ToInt64(comment.Id),
                        OwnerId = Convert.ToInt64(comment.OwnerId),
                    });
                    likes++;
                }
            }

            return likes;
        }

        private void BanDiedFriends()
        {
            Console.WriteLine("Бан мёртвых друзей");
            VkCollection<User>  friends;
            int offset = 0;
            int countBans = 0;
            const int COUNT_USER = 5000;
            do
            {
                friends = api.Friends.Get(new FriendsGetParams
                {
                    Count = COUNT_USER,
                    Offset = offset,
                });

                offset += friends.Count;
                var users = api.Users.Get(friends.Select(user => user.Id).ToArray());
                foreach (var user in users)
                {
                    if (user.Deactivated != Deactivated.Activated)
                    {
                        api.Account.Ban(user.Id);
                        countBans++;
                    }
                }
            }
            while (friends.Count == COUNT_USER);
            Console.WriteLine($"Количество забаненых {countBans}");
        } 

        public void Start(string groupId, int numbPeople, string clearFriends)
        {
            if( clearFriends.ToUpper() == "Y")
            {
                BanDiedFriends();
            }
            else
            {
                if (numbPeople > 0)
                {
                    WorkWithFriends();
                }
                else
                {
                    WorkWithPosts(groupId);
                }
            }
        }
    }
}
