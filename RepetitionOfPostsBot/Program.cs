using MyCustomClasses;

using RepetitionOfPostsBot.BotTask;

namespace RepetitionOfPostsBot
{
    internal static class Program
    {

        private static void Main()
        {
            var accessTokens = GosUslugi.GetAccessTokens();
            var task = new VKTask("@anime_art_for_every_day", 220199532, -1002066495859, ["Угадайка"], accessTokens);
            while (true)
            {
                task.RunAll();

                Thread.Sleep(TimeSpan.FromHours(1));
            }
        }
    }
}