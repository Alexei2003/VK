using Microsoft.VisualBasic;
using System.Collections.ObjectModel;
using System.Drawing;
using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.StringEnums;
using VkNet.Model;
using VkNet.Utils;

namespace MyCustomClasses
{
    public class VkApiCustom
    {
        private readonly TimeSpan TIME_SLEEP = TimeSpan.FromSeconds(2);
        public VkApi ApiOriginal { get; } = new();

        public void Authorize(IApiAuthParams @params)
        {
            while (true)
            {
                try
                {
                    ApiOriginal.Authorize(@params);
                    return;
                }
                catch(VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public bool AccountSetOnline(bool? voip = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Account.SetOnline(voip);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public WallGetObject WallGet(WallGetParams @params, bool skipAuthorization = false)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Wall.Get(@params, skipAuthorization);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public RepostResult WallRepost(string @object, string message, long? groupId, bool markAsAds)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Wall.Repost(@object, message, groupId, markAsAds);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public long LikesAdd(LikesAddParams @params)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Likes.Add(@params);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public VkCollection<User> FriendsGetSuggestions(FriendsFilter? filter = null, long? count = null, long? offset = null, UsersFields fields = null, NameCase? nameCase = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Friends.GetSuggestions(filter, count, offset, fields, nameCase);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public AddFriendStatus? FriendsAdd(long userId, string text = "", bool? follow = null)
        {
            int i = 10;
            while (i > 0)
            {
                try
                {
                    i--;
                    var res = ApiOriginal.Friends.Add(userId, text, follow);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
            return null;
        }

        public GetRequestsResult FriendsGetRequests(FriendsGetRequestsParams @params)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Friends.GetRequests(@params);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public bool AccountBan(long ownerId)
        {
            while (true)
            {
                try
                {

                    var res = ApiOriginal.Account.Ban(ownerId);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
                catch (Exception ex)
                {
                    if(ex.Message.Contains("Access denied: user already blacklisted"))
                    {
                        return true;
                    }
                }
            }
        }

        public long WallCreateComment(WallCreateCommentParams @params)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Wall.CreateComment(@params);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public WallGetCommentsResult WallGetComments(WallGetCommentsParams @params, bool skipAuthorization = false)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Wall.GetComments(@params, skipAuthorization);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public bool StatsTrackVisitor()
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Stats.TrackVisitor();
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public UploadServerInfo PhotoGetWallUploadServer(long? groupId = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Photo.GetWallUploadServer(groupId);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public ReadOnlyCollection<Photo> PhotoSaveWallPhoto(string response, ulong? userId, ulong? groupId = null, string caption = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Photo.SaveWallPhoto(response, userId, groupId, caption);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public long WallPost(WallPostParams @params)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Wall.Post(@params);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public VkCollection<User> FriendsGet(FriendsGetParams @params, bool skipAuthorization = false)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Friends.Get(@params, skipAuthorization);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public VkCollection<Group> UsersGetSubscriptions(long? userId = null, int? count = null, int? offset = null, GroupsFields fields = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Users.GetSubscriptions(userId, count, offset, fields);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public VkCollection<User> UsersGetFollowers(long? userId = null, int? count = null, int? offset = null, ProfileFields fields = null, NameCase? nameCase = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Users.GetFollowers(userId, count, offset, fields, nameCase);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
                catch (Exception ex)
                {
                    if(ex.Message.Contains("Index was out of range. Must be non-negative and less than the size of the collection."))
                    {
                        return null;
                    }
                }
            }
        }

        public ReadOnlyCollection<User> UsersGet(IEnumerable<long> userIds, ProfileFields fields = null, NameCase? nameCase = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Users.Get(userIds,fields,nameCase);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }
    }
}