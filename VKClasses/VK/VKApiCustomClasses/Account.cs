using VkNet;

namespace VKClasses.VK.VKApiCustomClasses
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
                    return ApiOriginal.Account.SetOnline(voip);
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

                    return ApiOriginal.Account.Ban(ownerId);
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
