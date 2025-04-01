using VkNet;
using VkNet.Model;

namespace VKClasses.VK.VKApiCustomClasses
{
    public class Newsfeed
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Newsfeed(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public NewsSearchResult Search(NewsFeedSearchParams @params)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.NewsFeed.Search(@params);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }
    }
}
