using MyCustomClasses;
using VkNet.Model;

namespace AddPost.Classes
{
    internal class Authorize
    {
        public VkApiCustom Api { get; } = new();
        public Authorize(string accessToken)
        {
            Api.Authorize(new ApiAuthParams
            {
                AccessToken = accessToken
            });
            Api.StatsTrackVisitor();
        }
    }
}
