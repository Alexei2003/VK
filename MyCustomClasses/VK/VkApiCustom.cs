using MyCustomClasses.VK.VKApiCustomClasses;
using VkNet;
using VkNet.Model;

namespace MyCustomClasses.VK
{
    public class VkApiCustom
    {
        private readonly TimeSpan TIME_SLEEP;

        public VkApi ApiOriginal { get; }
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
        public Stories Stories { get; }
        public ShortVideo ShortVideo { get; }

        public VkApiCustom()
        {
            TIME_SLEEP = TimeSpan.FromSeconds(2);

            ApiOriginal = new();
            Polls = new Polls(ApiOriginal, TIME_SLEEP);
            Account = new(ApiOriginal, TIME_SLEEP);
            Friends = new(ApiOriginal, TIME_SLEEP);
            Groups = new(ApiOriginal, TIME_SLEEP);
            Likes = new(ApiOriginal, TIME_SLEEP);
            Photo = new(ApiOriginal, TIME_SLEEP);
            Stats = new(ApiOriginal, TIME_SLEEP);
            Users = new(ApiOriginal, TIME_SLEEP);
            Wall = new(ApiOriginal, TIME_SLEEP);
            Newsfeed = new(ApiOriginal, TIME_SLEEP);
            Stories = new(ApiOriginal, TIME_SLEEP);
            ShortVideo = new(ApiOriginal, TIME_SLEEP);
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
                    ApiOriginal.Authorize(@params);
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