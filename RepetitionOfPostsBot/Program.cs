using MyCustomClasses;
using RepetitionOfPostsBot.BotTask;

namespace RepetitionOfPostsBot
{
    internal static class Program
    {

        private static void Main()
        {
            var accessTokens = GosUslugi.GetAccessTokens();

            var threadRepeatVKPosts = new Thread(new ParameterizedThreadStart(VKTask.RepeatVKPosts));
            threadRepeatVKPosts.Start(accessTokens.GetValueOrDefault(GosUslugi.VK));

            var threadSendVkPostToOther = new Thread(new ParameterizedThreadStart(VKTask.SendVkPostToOther));
            threadSendVkPostToOther.Start(accessTokens);

            while (true)
            {
                Console.WriteLine("RepetitionOfPostsBot work " + DateTime.Now.ToString());
                Thread.Sleep(TimeSpan.FromHours(1));
            }
        }
    }
}