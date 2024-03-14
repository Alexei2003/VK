using VkNet;
using VkNet.Model;

namespace VkApiCustom.VkApiCustomClasses
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
                    var res = ApiOriginal.Wall.Get(@params, skipAuthorization);
                    return res;
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
                    var res = ApiOriginal.Wall.Repost(@object, message, groupId, markAsAds);
                    return res;
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
                    var res = ApiOriginal.Wall.CreateComment(@params);
                    return res;
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
                    var res = ApiOriginal.Wall.GetComments(@params, skipAuthorization);
                    return res;
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
                    var res = ApiOriginal.Wall.Post(@params);
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
