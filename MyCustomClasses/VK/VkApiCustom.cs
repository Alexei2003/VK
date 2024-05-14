using MyCustomClasses.VK.VKApiCustomClasses;
using VkNet;
using VkNet.Model;

namespace MyCustomClasses.VK
{
    public class VkApiCustom
    {
        private readonly TimeSpan TIME_SLEEP;
        public long? UserId { get; private set; }

        private readonly VkApi apiOriginal;
        public Account Account { get; }
        public Friends Friends { get; }
        public Groups Groups { get; }
        public VKApiCustomClasses.Likes Likes { get; }
        public VKApiCustomClasses.Photo Photo { get; }
        public Stats Stats { get; }
        public Users Users { get; }
        public VKApiCustomClasses.Wall Wall { get; }
        public Newsfeed Newsfeed { get; }
        public Polls Polls { get; }

        public VkApiCustom()
        {
            TIME_SLEEP = TimeSpan.FromSeconds(2);

            apiOriginal = new();
            Polls = new Polls(apiOriginal, TIME_SLEEP);
            Account = new(apiOriginal, TIME_SLEEP);
            Friends = new(apiOriginal, TIME_SLEEP);
            Groups = new(apiOriginal, TIME_SLEEP);
            Likes = new(apiOriginal, TIME_SLEEP);
            Photo = new(apiOriginal, TIME_SLEEP);
            Stats = new(apiOriginal, TIME_SLEEP);
            Users = new(apiOriginal, TIME_SLEEP);
            Wall = new(apiOriginal, TIME_SLEEP);
            Newsfeed = new(apiOriginal, TIME_SLEEP);
        }

        public VkApiCustom(string accessToken) : this()
        {
            AuthorizeAndTrackAndOnline(accessToken);
        }

        public void Authorize(IApiAuthParams @params)
        {
            while (true)
            {
                try
                {
                    apiOriginal.Authorize(@params);
                    UserId = apiOriginal.UserId;
                    return;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public void AuthorizeAndTrackAndOnline(string accessToken)
        {
            Authorize(new ApiAuthParams
            {
                AccessToken = accessToken
            });
            Stats.TrackVisitor();
            Account.SetOnline(false);
        }
    }
}