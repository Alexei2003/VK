using Newtonsoft.Json;

using VkNet;
using VkNet.Model;

namespace VKClasses.VK.VKApiCustomClasses
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

        private class ResponseClass
        {
            public WallGetObject Response { get; set; } = new();
        }

        public WallGetObject Get(WallGetParams @params, bool skipAuthorization = false)
        {
            while (true)
            {
                try
                {
                    //return ApiOriginal.Wall.Get(@params, skipAuthorization);

                    var response = ApiOriginal.Invoke("wall.get",
                    new Dictionary<string, string>
                    {
                        {"v","5.199"},
                        {"access_token", ApiOriginal.Token},
                        {"domain",  @params.OwnerId?.ToString() ?? ""},
                        {"offset", @params.Offset.ToString()},
                        {"count", @params.Count.ToString()},
                        {"filter", @params.Filter?.ToString() ?? ""},
                        {"extended", @params.Extended.ToString()},
                        {"fields", @params.Fields?.ToString() ?? ""},
                    });
                    response = response.Replace("base", "z");
                    var responseClass = JsonConvert.DeserializeObject<ResponseClass>(response);
                    return responseClass.Response;
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
