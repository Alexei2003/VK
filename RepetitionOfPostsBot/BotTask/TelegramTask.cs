using Telegram.Bot;
using Telegram.Bot.Types;

namespace RepetitionOfPostsBot.BotTask
{
    internal static class TelegramTask
    {
        private const long CHAT_ID = -1002066495859;

        public static async Task PushPost(string accessToken, string caption, Uri[] imagesUrls)
        {
            var botClient = new TelegramBotClient(accessToken);

            var mediaArr = new InputMediaPhoto[imagesUrls.Length];

            for (var i = 0; i < imagesUrls.Length; i++)
            {
                mediaArr[i] = new InputMediaPhoto(InputFile.FromUri(imagesUrls[i]));
            }

            mediaArr.First().Caption = caption;

            var message = await botClient.SendMediaGroupAsync
            (
                chatId: CHAT_ID,
                media: mediaArr
            );
        }
    }
}
