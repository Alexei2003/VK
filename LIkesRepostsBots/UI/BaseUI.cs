using LikesRepostsBots.Classes;

namespace LikesRepostsBots.UI
{
    public class BaseUI
    {
        protected static BotsList? _botList = null;
        public static void Initialize()
        {
            var accessTokensAndNames = File.ReadAllLines(Path.Combine("AccessTokens.txt"));
            _botList = new BotsList(accessTokensAndNames);
        }
    }
}
