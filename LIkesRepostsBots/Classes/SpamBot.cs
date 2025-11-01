using Other;

using VKClasses.VK;

using VkNet.Enums.StringEnums;
using VkNet.Model;
using VkNet.Utils;

using static LikesRepostsBots.Classes.BotsWorksParams;

namespace LikesRepostsBots.Classes
{
    public sealed class SpamBot
    {
        private readonly VkApiCustom _api;
        private const int CHANCE_LIKE_COMMENTS = 5;
        private const int CHANCE_REPOST = 5;
        private const int MAX_COUNT_POST = 24;
        private readonly string _accessToken;

        public string BotName { get; set; }

        public SpamBot(string botName, string accessToken)
        {
            _api = new();
            BotName = botName;
            this._accessToken = accessToken;
        }

        private void Authorize()
        {
            _api.AuthorizeAndTrackAndOnline(_accessToken);
            SleepAfterAction();
        }

        private static void SleepAfterAction()
        {
            Thread.Sleep(5 * RandomStatic._1SECOND + RandomStatic.Rand.Next(10 * RandomStatic._1SECOND));
        }

        private void WorkWithPosts(long groupId)
        {
            var botWall = _api.Wall.Get(new WallGetParams
            {
                Count = 1,
                Filter = WallFilter.All
            });
            SleepAfterAction();

            var wall = _api.Wall.Get(new WallGetParams
            {
                OwnerId = -1 * groupId,
                Count = MAX_COUNT_POST,
                Filter = WallFilter.All
            });
            SleepAfterAction();

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
            for (int numbPost = countPosts - 1; numbPost > -1; numbPost--)
            {
                // Лайк репост
                if (RandomStatic.Rand.Next(CHANCE_REPOST) == 0 && !wall.WallPosts[numbPost].Text.Contains('!'))
                {
                    repostResult = _api.Wall.Repost("wall" + wall.WallPosts[numbPost].OwnerId + "_" + wall.WallPosts[numbPost].Id, "", null, false);
                    SleepAfterAction();
                    _api.Likes.Add(new LikesAddParams
                    {
                        Type = LikeObjectType.Post,
                        ItemId = Convert.ToInt64(repostResult.PostId),
                    });
                    SleepAfterAction();
                }
                else
                {
                    // Лайк оригинала
                    _api.Likes.Add(new LikesAddParams
                    {
                        Type = LikeObjectType.Post,
                        OwnerId = wall.WallPosts[numbPost].OwnerId,
                        ItemId = Convert.ToInt64(wall.WallPosts[numbPost].Id),
                    });
                    SleepAfterAction();
                }

                // Выбор в опросе
                foreach (var attachment in wall.WallPosts[numbPost].Attachments)
                {
                    if (attachment.Type == typeof(Poll))
                    {
                        var poll = (Poll)attachment.Instance;
                        var index = RandomStatic.Rand.Next(poll.Answers.Count - 1);

                        _api.Polls.AddVote(new PollsAddVoteParams
                        {
                            OwnerId = poll.OwnerId,
                            PollId = poll.Id.Value,
                            AnswerId = poll.Answers[index].Id.Value
                        });
                        SleepAfterAction();
                    }
                }

                // Лайк комментов
                AddCommentsLike(groupId, wall.WallPosts[numbPost].Id);
            }
        }

        private void AddCommentsLike(long groupId, long? postId)
        {
            var comments = _api.Wall.GetComments(new WallGetCommentsParams
            {
                OwnerId = -1 * groupId,
                PostId = Convert.ToInt64(postId),
                Count = 100,
            });
            SleepAfterAction();

            foreach (var comment in comments.Items)
            {
                int randNumb = RandomStatic.Rand.Next(CHANCE_LIKE_COMMENTS);
                if (randNumb == 0)
                {
                    _api.Likes.Add(new LikesAddParams
                    {
                        Type = LikeObjectType.Comment,
                        ItemId = comment.Id,
                        OwnerId = comment.OwnerId,
                    });
                    SleepAfterAction();
                }
            }
        }

        private void WorkWithFriends(BotsWorksParams botsParams)
        {
            var addCountFriends = botsParams.AddFriendsCount;

            ClearSpaceForFriends(addCountFriends);

            var friends = _api.Friends.Get(new FriendsGetParams
            {
                Order = FriendsOrder.Random
            });

            if (friends.TotalCount > 10 || botsParams.GroupIdForGood == null)
            {
                AddToFriendsFromRecomendedList(addCountFriends);
            }
            else
            {
                AddToFriendsFromGoodGroup(addCountFriends, botsParams.GroupIdForGood.Value);
            }
        }

        private void ClearSpaceForFriends(int addCountFriends)
        {
            var friends = _api.Friends.Get(new FriendsGetParams
            {
                Order = FriendsOrder.Random
            });
            SleepAfterAction();

            var sendRequests = _api.Friends.GetRequests(new FriendsGetRequestsParams
            {
                Out = true
            });
            SleepAfterAction();

            var totalCount = friends.TotalCount + sendRequests.Count;

            if (totalCount > 9000)
            {
                if (sendRequests.Count > 100)
                {
                    for (int i = 0; i < addCountFriends; i++)
                    {
                        _api.Account.Ban(sendRequests.Items[i]);
                        SleepAfterAction();
                    }
                }
                else
                {
                    for (int i = 0; i < addCountFriends; i++)
                    {
                        _api.Account.Ban(friends[i].Id);
                        SleepAfterAction();
                    }
                }
            }
        }

        private void AddToFriendsFromRecomendedList(int addCountFriends)
        {
            int offset = 0;
            var suggestions = _api.Friends.GetSuggestions(offset: offset);
            SleepAfterAction();

            int index = 0;
            int numbNewFriends = 0;
            while (numbNewFriends < addCountFriends)
            {
                if (index >= suggestions.Count)
                {
                    offset += suggestions.Count;
                    suggestions = _api.Friends.GetSuggestions(offset: offset);
                    SleepAfterAction();
                    index = 0;

                    if (suggestions.Count == 0)
                    {
                        return;
                    }
                }

                if (AddToFriends(suggestions[index].Id))
                {
                    numbNewFriends++;
                }

                index++;
            }
        }

        private void AddToFriendsFromGoodGroup(int addCountFriends, long groupId)
        {
            int offset = 0;
            var members = _api.Groups.GetMembers(new GroupsGetMembersParams
            {
                GroupId = groupId.ToString()
            });
            SleepAfterAction();

            int index = RandomStatic.Rand.Next(30);
            int numbNewFriends = 0;
            while (numbNewFriends < addCountFriends)
            {
                if (index >= members.Count)
                {
                    offset += members.Count;
                    members = _api.Groups.GetMembers(new GroupsGetMembersParams
                    {
                        GroupId = groupId.ToString()
                    });
                    SleepAfterAction();
                    index = 0;

                    if (members.Count == 0)
                    {
                        offset = 0;
                        continue;
                    }
                }

                if (AddToFriends(members[index].Id, true))
                {
                    numbNewFriends++;
                }

                index += (1 + RandomStatic.Rand.Next(30));
            }
        }

        private bool AddToFriends(long userId, bool skipPeopleCheck = false)
        {
            if (PeoplesList.Add(userId) || skipPeopleCheck)
            {
                if (!IsMassAccountSleep(userId) && _api.Friends.Add(userId) != null)
                {
                    SleepAfterAction();
                    return true;
                }
                else
                {
                    _api.Account.Ban(userId);
                    SleepAfterAction();
                }
            }
            else
            {
                _api.Account.Ban(userId);
                SleepAfterAction();
            }

            return false;
        }

        private bool IsMassAccountSleep(long personId)
        {

            var flag = IsMassAccount(personId);
            SleepAfterAction();
            return flag;
        }

        private bool IsMassAccount(long personId)
        {
            const int COUNT_FRIENDS = 300;
            const int COUNT_FOLLOWING = 300;
            const int COUNT_FOLOWERS = 300;

            var user = _api.Users.Get(new long[] { personId });
            if (user[0].IsClosed == true)
            {

                return true;
            }

            var friends = _api.Friends.Get(new FriendsGetParams
            {
                UserId = personId,
            });
            if (friends != null && friends.TotalCount > COUNT_FRIENDS)
            {
                return true;
            }

            var following = _api.Users.GetSubscriptions(personId);
            if (following != null && following.TotalCount > COUNT_FOLLOWING)
            {
                return true;
            }

            var followers = _api.Users.GetFollowers(personId);
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

        private void BanDiedAndMassFriends(ClearFriendsType clearFriends)
        {
            VkCollection<User> friends;
            int offset = 0;
            const int COUNT_USER = 5000;
#if DEBUG
            var count = 0;
            var countBan = 0;
#endif
            do
            {
                friends = _api.Friends.Get(new FriendsGetParams
                {
                    Count = COUNT_USER,
                    Offset = offset,
                });
                offset += friends.Count;

                var users = _api.Users.Get(friends.Select(user => user.Id).ToArray());
                SleepAfterAction();
                foreach (var user in users)
                {
                    if (user.Id == 713712954 || user.Id == 338992901)
                    {
                        continue;
                    }

                    if (user.Deactivated != Deactivated.Activated)
                    {
                        _api.Account.Ban(user.Id);
#if DEBUG
                        countBan++;
#endif
                        SleepAfterAction();
                    }
                    else
                    {
                        if (clearFriends == ClearFriendsType.BanAndMathAccount && IsMassAccount(user.Id))
                        {
                            _api.Account.Ban(user.Id);
#if DEBUG
                            countBan++;
#endif
                            SleepAfterAction();
                        }
                    }
#if DEBUG
                    count++;
                    Console.WriteLine($"{countBan}/{count}");
#endif
                }

            }
            while (friends.Count == COUNT_USER);
        }

        private void BanPeopleFromGroup(long groupId)
        {
            VkCollection<User> members;
            int offset = 0;
            const int COUNT_USER = 1000;
            do
            {
                members = _api.Groups.GetMembers(new GroupsGetMembersParams
                {
                    GroupId = groupId.ToString(),
                    Offset = offset,
                    Count = COUNT_USER
                });
                SleepAfterAction();
                offset += members.Count;

                foreach (var member in members)
                {
                    PeoplesList.Add(member.Id);
                }
            }
            while (members.Count == COUNT_USER);
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
                            if (botParams.MakeRepost && botParams.GroupIdForGood != null)
                            {
                                WorkWithPosts(botParams.GroupIdForGood.Value);
                            }
                            break;
                        case 1:
                            if (botParams.AddFriendsCount > 0)
                            {
                                WorkWithFriends(botParams);
                            }
                            break;
                        case 2:
                            if (botParams.ClearFriends != BotsWorksParams.ClearFriendsType.None)
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

        private static int[] MixAction()
        {
            var action = new int[] { 0, 1, 2, 3 };
            int n = action.Length;
            while (n > 1)
            {
                int k = RandomStatic.Rand.Next(n--);
                (action[n], action[k]) = (action[k], action[n]);
            }
            return action;
        }
    }
}
