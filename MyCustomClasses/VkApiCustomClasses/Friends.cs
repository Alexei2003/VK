using VkNet;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Enums.StringEnums;
using VkNet.Model;
using VkNet.Utils;

namespace MyCustomClasses.VkApiCustomClasses
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

        public VkCollection<User> GetSuggestions(FriendsFilter? filter = null, long? count = null, long? offset = null, UsersFields fields = null, NameCase? nameCase = null)
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

        public AddFriendStatus? Add(long userId, string text = "", bool? follow = null)
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

        public GetRequestsResult GetRequests(FriendsGetRequestsParams @params)
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

        public VkCollection<User> Get(FriendsGetParams @params, bool skipAuthorization = false)
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
    }
}
