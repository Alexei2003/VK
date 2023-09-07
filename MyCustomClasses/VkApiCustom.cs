using MyCustomClasses.VkApiCustomClasses;
using VkNet;
using VkNet.Model;

namespace MyCustomClasses
{
    public class VkApiCustom
    {
        private readonly TimeSpan TIME_SLEEP = TimeSpan.FromSeconds(2);
        public VkApi ApiOriginal { get; }

        public Account Account { get; }
        public VkApiCustomClasses.Wall Wall { get; }
        public VkApiCustomClasses.Likes Likes { get; }
        public Friends Friends { get; }
        public VkApiCustomClasses.Photo Photo { get; }
        public Users Users { get; }
        public Stats Stats { get; }

        public VkApiCustom()
        {
            ApiOriginal = new();
            Account = new(ApiOriginal, TIME_SLEEP);
            Wall = new(ApiOriginal, TIME_SLEEP);
            Likes = new(ApiOriginal, TIME_SLEEP);
            Friends = new(ApiOriginal, TIME_SLEEP);
            Photo = new(ApiOriginal, TIME_SLEEP);
            Users = new(ApiOriginal, TIME_SLEEP);
            Stats = new(ApiOriginal, TIME_SLEEP);
        }

        public void Authorize(IApiAuthParams @params)
        {
            while (true)
            {
                try
                {
                    ApiOriginal.Authorize(@params);
                    return;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

    }
}