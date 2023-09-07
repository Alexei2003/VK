using MyCustomClasses.VkApiCustomClasses;
using VkNet;
using VkNet.Model;

namespace MyCustomClasses
{
    public class VkApiCustom
    {
        private readonly TimeSpan TIME_SLEEP = TimeSpan.FromSeconds(2);
        public VkApi ApiOriginal { get; } = new();

        public Account Account { get; }
        public VkApiCustomClasses.Wall Wall { get; }
        public VkApiCustomClasses.Likes Likes { get; }
        public Friends Friends { get; }
        public VkApiCustomClasses.Photo Photo { get; }
        public Users Users { get; }
        public Stats Stats { get; }

        public VkApiCustom()
        {
            Account Account = new(ApiOriginal, TIME_SLEEP);
            VkApiCustomClasses.Wall Wall = new(ApiOriginal, TIME_SLEEP);
            VkApiCustomClasses.Likes Likes = new(ApiOriginal, TIME_SLEEP);
            Friends Friends = new(ApiOriginal, TIME_SLEEP);
            VkApiCustomClasses.Photo Photo = new(ApiOriginal, TIME_SLEEP);
            Users Users = new(ApiOriginal, TIME_SLEEP);
            Stats Stats = new(ApiOriginal, TIME_SLEEP);
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