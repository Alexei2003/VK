using VkNet;

namespace MyCustomClasses.VK.VKApiCustomClasses
{
    public class ShortVideo
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public ShortVideo(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }
    }
}
