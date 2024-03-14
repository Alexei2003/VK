using VkNet;

namespace VkApiCustom.VkApiCustomClasses
{
    public class Account
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Account(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public bool SetOnline(bool? voip = null)
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

        public bool Ban(long ownerId)
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
                    if (ex.Message.Contains("Access denied: user already blacklisted"))
                    {
                        return true;
                    }
                }
            }
        }
    }
}
