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
                    var res = ApiOriginal.Stats.TrackVisitor();
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
