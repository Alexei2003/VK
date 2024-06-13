using MyCustomClasses.Tags.Editors;
using Telegram.Bot;
using Telegram.Bot.Types;
using VkNet.Model;

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


            caption = BaseTagsEditor.RemoveBaseTags(caption);

            var tags = caption.Split('#', StringSplitOptions.RemoveEmptyEntries);

            caption = "";

            for (var i=0;i<tags.Length; i++)
            {
                caption += tags[i].Split('@', StringSplitOptions.RemoveEmptyEntries).First();
            }

            mediaArr.First().Caption = caption;

            var message = botClient.SendMediaGroupAsync
            (
                chatId: CHAT_ID,
                media: mediaArr
            );
        }
    }
}
