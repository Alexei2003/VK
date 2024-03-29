using VkNet;

namespace MyCustomClasses.VkApiCustomClasses
{
    public class Stats
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Stats(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public bool TrackVisitor()
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.Stats.TrackVisitor();
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }
    }
}
