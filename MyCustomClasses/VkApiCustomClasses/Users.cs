using System.Collections.ObjectModel;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.StringEnums;
using VkNet.Model;
using VkNet.Utils;

namespace VkApiCustom.VkApiCustomClasses
{
    public class Users
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Users(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public VkCollection<Group> GetSubscriptions(long? userId = null, int? count = null, int? offset = null, GroupsFields fields = null)
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
                catch (Exception e) when (e is VkNet.Exception.AccessDeniedException)
                {
                    return null;
                }
            }
        }

        public VkCollection<User> GetFollowers(long? userId = null, int? count = null, int? offset = null, ProfileFields fields = null, NameCase? nameCase = null)
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
                catch (Exception e) when (e is VkNet.Exception.AccessDeniedException)
                {
                    return null;
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Index was out of range. Must be non-negative and less than the size of the collection."))
                    {
                        return null;
                    }
                }
            }
        }

        public ReadOnlyCollection<User> Get(IEnumerable<long> userIds, ProfileFields fields = null, NameCase? nameCase = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Users.Get(userIds, fields, nameCase);
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
