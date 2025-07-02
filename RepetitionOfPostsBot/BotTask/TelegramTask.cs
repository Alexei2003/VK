using Telegram.Bot;
using Telegram.Bot.Types;

namespace RepetitionOfPostsBot.BotTask
{
    public static class TelegramTask
    {
        public static async Task PushPost(long chatId, string accessToken, string caption, Uri[] imagesUrls)
        {
            var botClient = new TelegramBotClient(accessToken);

            var mediaArr = new InputMediaPhoto[imagesUrls.Length];

            for (var i = 0; i < imagesUrls.Length; i++)
            {
                mediaArr[i] = new InputMediaPhoto(InputFile.FromUri(imagesUrls[i]));
            }

            mediaArr[0].Caption = caption;

            await botClient.SendMediaGroup
            (
                chatId: chatId,
                media: mediaArr
            );
        }
    }
}
