using VkNet;
using VkNet.Model;

namespace MyCustomClasses.VK.VKApiCustomClasses
{
    public class Polls
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Polls(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public bool AddVote(PollsAddVoteParams @params)
        {
            while (true)
            {
                try
                {
                    return ApiOriginal.PollsCategory.AddVote(@params);
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }
    }
}
