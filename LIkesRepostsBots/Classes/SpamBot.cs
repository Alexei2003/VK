using MyCustomClasses;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace LikesRepostsBots.Classes
{
    internal class SpamBot
    {
        private readonly VkApiCustom api = new();
        private readonly Random rand = new();
        private readonly int randNumbCreateComment;
        private const int CHANCE_COMMENT = 10;
        private const int CHANCE_LIKE = 5;

        public SpamBot(string accessToken)
        {
            api.Authorize(new ApiAuthParams
            {
                AccessToken = accessToken
            });
            api.Account.SetOnline(false);
            randNumbCreateComment = rand.Next(CHANCE_COMMENT);
        }

        private void WorkWithPosts(string groupId)
        {
            var wall = api.Wall.Get(new WallGetParams
            {
                OwnerId = -1 * Convert.ToInt32(groupId),
                Count = 7,
                Filter = WallFilter.All
            });

            var botWall = api.Wall.Get(new WallGetParams
            {
                OwnerId = api.ApiOriginal.UserId,
                Count = 1,
                Filter = WallFilter.All
            });

            long? countPosts;
            if (botWall.WallPosts[0].CopyHistory != null)
            {

                for (countPosts = 0; countPosts < 7; countPosts++)
                {
                    if (wall.WallPosts[Convert.ToInt32(countPosts)].Id == botWall.WallPosts[0].CopyHistory[0].Id)
                    {
                        break;
                    }
                }
            }
            else
            {
                countPosts = 7;
            }

            RepostResult repostResult;
            var rand = new Random();
            for (long? numbPost = countPosts - 1; numbPost > 3;)
            {
                Thread.Sleep(TimeSpan.FromSeconds(rand.Next(1)));

                repostResult = api.Wall.Repost("wall" + wall.WallPosts[0].OwnerId + "_" + wall.WallPosts[Convert.ToInt32(numbPost)].Id, "", api.ApiOriginal.UserId, false);

                int likes = AddCommentsLike(groupId, wall.WallPosts[Convert.ToInt32(numbPost)].Id);
                if (likes > 0)
                {
                    Console.WriteLine($"Число лайкнутых комментариев {likes}");
                }
                /*
                                if (CreatesComments(groupId, wall.WallPosts[Convert.ToInt32(numbPost)].Id))
                                {
                                    Console.WriteLine("Добавление комментария");
                                }
                */
                api.Likes.Add(new LikesAddParams
                {
                    Type = LikeObjectType.Post,
                    ItemId = Convert.ToInt64(repostResult.PostId),
                });

                Console.WriteLine($"{countPosts - numbPost}/{countPosts - 4}");
                numbPost--;
            }
        }

        private void WorkWithFriends()
        {
            PeopleDictionary people = new();
            people.Read();

            AddToFriendsFromRecomendedList(people);
            //AddToPeopleDictionaryFromRequests(people,false);

            people.Write();
        }

        private void AddToFriendsFromRecomendedList(PeopleDictionary people)
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

        private void AddToPeopleDictionaryFromRequests(PeopleDictionary people, bool addToFriends)
        {
            Console.WriteLine("Рассмотр заявок");
            var requests = api.Friends.GetRequests(new FriendsGetRequestsParams
            {
                Count = 10,
                Out = false
            });

            foreach (var personId in requests.Items)
            {

                if (addToFriends)
                {
                    var res = api.Friends.Add(personId);
                    if (res == null)
                    {
                        api.Account.Ban(personId);
                    }
                }
                people.AddNotContains(personId);

            }
        }

        private bool CreatesComments(string groupId, long? postId)
        {

            int randNumb = rand.Next(CHANCE_COMMENT);

            if (randNumb == randNumbCreateComment)
            {
                var comments = new CommentsDictionary();
                randNumb = rand.Next(comments.Count);

                api.Wall.CreateComment(new WallCreateCommentParams
                {
                    OwnerId = -1 * Convert.ToInt32(groupId),
                    PostId = Convert.ToInt64(postId),
                    Message = comments[randNumb],
                });
                return true;
            }
            return false;
        }

        private int AddCommentsLike(string groupId, long? postId)
        {
            var rand = new Random();

            var comments = api.Wall.GetComments(new WallGetCommentsParams
            {
                OwnerId = -1 * Convert.ToInt32(groupId),
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

        public void Start(string groupId, int answer)
        {
            WorkWithPosts(groupId);
            if (answer != 0)
            {
                WorkWithFriends();
            }
        }
    }
}
