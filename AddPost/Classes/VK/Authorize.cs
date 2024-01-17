using MyCustomClasses;
using VkNet.Model;

namespace AddPost.Classes.VK
{
    internal sealed class Authorize
    {
        public VkApiCustom Api { get; } = new();
        public Authorize(string accessToken)
        {
            Api.Authorize(new ApiAuthParams
            {
                AccessToken = accessToken
            });
            Api.Stats.TrackVisitor();
        }
    }
}
