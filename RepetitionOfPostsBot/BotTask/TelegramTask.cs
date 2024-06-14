using MyCustomClasses.Tags;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace RepetitionOfPostsBot.BotTask
{
    internal static class TelegramTask
    {
        private const long CHAT_ID = -1002066495859;

        public static void PushPost(string accessToken, string caption, Uri[] imagesUrl)
        {
            var botClient = new TelegramBotClient(accessToken);

            var mediaArr = new InputMediaPhoto[imagesUrl.Length];

            for (var i = 0; i < imagesUrl.Length; i++)
            {
                mediaArr[i] = new InputMediaPhoto(InputFile.FromUri(imagesUrl[i]));
            }

            mediaArr.First().Caption = TagsReplacer.ReplaceTagToTelegram(caption);

            var message = botClient.SendMediaGroupAsync
            (
                chatId: CHAT_ID,
                media: mediaArr
            );
        }
    }
}
