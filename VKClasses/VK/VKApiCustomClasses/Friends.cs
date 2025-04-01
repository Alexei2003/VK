using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.StringEnums;
using VkNet.Model;
using VkNet.Utils;

namespace VKClasses.VK.VKApiCustomClasses
{
    public class Friends
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Friends(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public VkCollection<User> GetSuggestions(FriendsFilter? filter = null, long? count = null, long? offset = null, UsersFields? fields = null, NameCase? nameCase = null)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Friends.GetSuggestions(filter, count, offset, fields, nameCase);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public AddFriendStatus? Add(long userId, string text = "", bool? follow = null)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Friends.Add(userId, text, follow);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
                catch (Exception e) when (e is VkNet.Exception.CannotAddUserBlacklistedException || e is VkNet.Exception.CannotAddYouBlacklistedException)
                {
                    return null;
                }
            }
        }

        public GetRequestsResult GetRequests(FriendsGetRequestsParams @params)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Friends.GetRequests(@params);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public VkCollection<User>? Get(FriendsGetParams @params, bool skipAuthorization = false)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Friends.Get(@params, skipAuthorization);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
                catch (Exception e) when (e is VkNet.Exception.AccessDeniedException)
                {
                    return null;
                }
            }
        }
    }
}
