using VkNet;
using VkNet.Model;
using VkNet.Utils;

namespace VkApiCustom.VkApiCustomClasses
{
    public class Groups
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Groups(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public VkCollection<User> GetMembers(GroupsGetMembersParams @params)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Groups.GetMembers(@params);
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
