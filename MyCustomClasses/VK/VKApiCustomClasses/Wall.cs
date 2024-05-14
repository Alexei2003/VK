using VkNet;
using VkNet.Model;

namespace MyCustomClasses.VK.VKApiCustomClasses
{
    public class Wall
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Wall(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public WallGetObject Get(WallGetParams @params, bool skipAuthorization = false)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Wall.Get(@params, skipAuthorization);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public RepostResult Repost(string @object, string message, long? groupId, bool markAsAds)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Wall.Repost(@object, message, groupId, markAsAds);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public long CreateComment(WallCreateCommentParams @params)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Wall.CreateComment(@params);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public WallGetCommentsResult GetComments(WallGetCommentsParams @params, bool skipAuthorization = false)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Wall.GetComments(@params, skipAuthorization);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public long Post(WallPostParams @params)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Wall.Post(@params);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }
    }
}
