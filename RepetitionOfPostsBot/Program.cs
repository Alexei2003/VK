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

            var threadCreateVkPostFromGelbooru = new Thread(new ParameterizedThreadStart(VKTask.CreateVkPostFromGelbooru));
            threadCreateVkPostFromGelbooru.Start(accessTokens.GetValueOrDefault(GosUslugi.VK));

            while (true)
            {
                Thread.Sleep(TimeSpan.FromHours(1));
            }
        }
    }
}