using VkNet;
using VkNet.Model;

namespace VKClasses.VK.VKApiCustomClasses
{
    public class Likes
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Likes(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public long Add(LikesAddParams @params)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Likes.Add(@params);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }
    }
}
