﻿using MyCustomClasses.VK;
using VkNet.Enums.StringEnums;
using VkNet.Model;
using VkNet.Utils;
using static LikesRepostsBots.Classes.BotsWorksParams;

namespace LikesRepostsBots.Classes
{
    internal sealed class SpamBot
    {
        private readonly VkApiCustom api;
        private readonly Random rand;
        private const int CHANCE_LIKE = 5;
        private const int CHANCE_REPOST = 5;
        private const int MAX_COUNT_POST = 16;
        private readonly PeoplesLIst people;
        private readonly string accessToken;

        public string BotName { get; set; }

        public SpamBot(string botName, string accessToken, PeoplesLIst people, Random rand)
        {
            api = new();
            this.people = people;
            this.rand = rand;
            BotName = botName;
            this.accessToken = accessToken;
        }

        private void Authorize()
        {
            api.AuthorizeAndTrackAndOnline(accessToken);
        }

        private void WorkWithPosts(long groupId)
        {
            var wall = api.Wall.Get(new WallGetParams
            {
                OwnerId = -1 * groupId,
                Count = MAX_COUNT_POST,
                Filter = WallFilter.All
            });

            var botWall = api.Wall.Get(new WallGetParams
            {
                OwnerId = api.UserId,
                Count = 1,
                Filter = WallFilter.All
            });

            int countPosts;
            if (botWall.WallPosts.Count > 0 && botWall.WallPosts[0].CopyHistory != null)
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
            for (int numbPost = countPosts - 1; numbPost > -1;)
            {
                // Лайк репост
                if (rand.Next(CHANCE_REPOST) == 0)
                {
                    repostResult = api.Wall.Repost("wall" + wall.WallPosts[numbPost].OwnerId + "_" + wall.WallPosts[numbPost].Id, "", api.UserId, false);
                    Console.WriteLine("репост");

                    api.Likes.Add(new LikesAddParams
                    {
                        Type = LikeObjectType.Post,
                        ItemId = Convert.ToInt64(repostResult.PostId),
                    });
                }
                // Лайк оригинала
                else
                {
                    api.Likes.Add(new LikesAddParams
                    {
                        Type = LikeObjectType.Post,
                        OwnerId = wall.WallPosts[numbPost].OwnerId,
                        ItemId = Convert.ToInt64(wall.WallPosts[numbPost].Id),
                    });
                }

                // Выбор в опросе
                foreach (var attachment in wall.WallPosts[numbPost].Attachments)
                {
                    if (attachment.Type == typeof(Poll))
                    {
                        var poll = (Poll)attachment.Instance;
                        var index = rand.Next(poll.Answers.Count - 1);

                        api.Polls.AddVote(new PollsAddVoteParams
                        {
                            OwnerId = poll.OwnerId,
                            PollId = poll.Id.Value,
                            AnswerId = poll.Answers[index].Id.Value
                        });

                    }
                }

                // Лайк комментов
                int likes = AddCommentsLike(groupId, wall.WallPosts[numbPost].Id);
                if (likes > 0)
                {
                    Console.WriteLine($"Число лайкнутых комментариев {likes}");
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
            const ulong countFriends = 1;

            var suggestions = api.Friends.GetSuggestions();

            int index = 0;
            int banCount = 0;
            int newUserCount = 0;
            for (ulong i = 0; i < countFriends && index < suggestions.Count; index++)
            {
                if (people.Add(suggestions[index].Id))
                {
                    if (!IsMassAccount(suggestions[index].Id) && api.Friends.Add(suggestions[index].Id) != null)
                    {
                        i++;
                    }
                    else
                    {
                        api.Account.Ban(suggestions[index].Id);
                        banCount++;
                    }
                    newUserCount++;
                }
                else
                {
                    api.Account.Ban(suggestions[index].Id);
                    banCount++;
                }

                AnimatedLoad();
            }
            Console.WriteLine();
            Console.WriteLine($"Количество забанненых аккаунтов {banCount}");
            Console.WriteLine($"Количество новых аккаунтов в списке  {newUserCount}");
        }

        private bool IsMassAccount(long personId)
        {
            const int COUNT_FRIENDS = 300;
            const int COUNT_FOLLOWING = 400;
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

            if (friends == null && following == null && followers == null)
            {
                return true;
            }

            return false;
        }

        private int AddCommentsLike(long groupId, long? postId)
        {
            var comments = api.Wall.GetComments(new WallGetCommentsParams
            {
                OwnerId = -1 * groupId,
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
                        ItemId = comment.Id,
                        OwnerId = comment.OwnerId,
                    });
                    likes++;
                }
            }

            return likes;
        }

        private void BanDiedAndMassFriends(ClearFriendsType clearFriends)
        {
            Console.WriteLine("Бан мёртвых друзей");
            VkCollection<User> friends;
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

                AnimatedLoad();

                var users = api.Users.Get(friends.Select(user => user.Id).ToArray());
                foreach (var user in users)
                {
                    if (user.Id == 713712954 || user.Id == 338992901)
                    {
                        continue;
                    }

                    if (user.Deactivated != Deactivated.Activated)
                    {
                        api.Account.Ban(user.Id);
                        countBans++;
                    }
                    else
                    {
                        if (clearFriends == ClearFriendsType.BanAndMathAccount && IsMassAccount(user.Id))
                        {
                            api.Account.Ban(user.Id);
                            countBans++;
                        }
                    }
                }
            }
            while (friends.Count == COUNT_USER);
            Console.WriteLine($"Количество забаненых {countBans}");
        }

        private void BanPeopleFromGroup(long groupId)
        {
            Console.WriteLine("Бан людей из мусор группы");
            VkCollection<User> members;
            int offset = 0;
            int countBans = 0;
            const int COUNT_USER = 1000;
            do
            {
                members = api.Groups.GetMembers(new GroupsGetMembersParams
                {
                    GroupId = groupId.ToString(),
                    Offset = offset,
                    Count = COUNT_USER
                });
                offset += members.Count;

                AnimatedLoad();

                foreach (var member in members)
                {
                    if (people.Add(member.Id))
                    {
                        countBans++;
                    }
                }
            }
            while (members.Count == COUNT_USER);
            Console.WriteLine($"Количество забаненых {countBans}");
        }

        private static void AnimatedLoad()
        {
            Console.Write("/");
        }

        public bool Start(BotsWorksParams botParams)
        {
            try
            {
                Authorize();

                var action = MixAction();

                foreach (var index in action)
                {
                    switch (index)
                    {
                        case 0:
                            if (botParams.MakeRepost == true && botParams.GroupIdForGood != null)
                            {
                                WorkWithPosts(botParams.GroupIdForGood.Value);
                            }
                            break;
                        case 1:
                            for (int i = 0; i < botParams.AddFriendsCount; i++)
                            {
                                WorkWithFriends();
                            }
                            break;
                        case 2:
                            if (botParams.ClearFriends > 0)
                            {
                                BanDiedAndMassFriends(botParams.ClearFriends);
                            }
                            break;
                        case 3:
                            if (botParams.BanPeopleFromGroup && botParams.GroupIdForBad != null)
                            {
                                BanPeopleFromGroup(botParams.GroupIdForBad.Value);
                            }
                            break;
                    }
                }
                return true;
            }
            catch (Exception e) when (e is VkNet.Exception.UserAuthorizationFailException || e is VkNet.Exception.VkApiException)
            {
                return false;
            }
        }

        private int[] MixAction()
        {
            var action = new int[] { 0, 1, 2, 3 };
            int n = action.Length;
            while (n > 1)
            {
                int k = rand.Next(n--);
                (action[n], action[k]) = (action[k], action[n]);
            }
            return action;
        }
    }
}
