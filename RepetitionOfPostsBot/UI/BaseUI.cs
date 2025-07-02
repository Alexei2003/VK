using RepetitionOfPostsBot.BotTask;

using VKClasses;

namespace RepetitionOfPostsBot.UI
{
    public class BaseUI
    {
        protected static VKTask? _task = null;
        public static void Initialize()
        {
            var accessTokens = GosUslugi.GetAccessTokens();
            _task = new VKTask("@anime_art_for_every_day", 220199532, -1002066495859, ["Угадайка"], accessTokens);
        }
    }
}
