using MyCustomClasses;
using RepetitionOfPostsBot.BotTask;

namespace RepetitionOfPostsBot
{
    internal static class Program
    {

        private static void Main()
        {
            var accessTokens = GosUslugi.GetAccessTokens();

            var RepeatVKPosts = new Thread(new ParameterizedThreadStart(VKTask.RepeatVKPosts));
            RepeatVKPosts.Start(accessTokens.GetValueOrDefault(GosUslugi.VK));

            var threadSendVkPostToOther = new Thread(new ParameterizedThreadStart(VKTask.SendVkPostToOther));
            threadSendVkPostToOther.Start(accessTokens);

            while (true)
            {
                Thread.Sleep(TimeSpan.FromDays(10));
            }
        }
    }
}