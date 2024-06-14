
using DSharpPlus;
using DSharpPlus.Entities;

namespace RepetitionOfPostsBot.BotTask
{
    internal class DiscordTask
    {
        private const ulong THREAD_ID = 1251120297050898473;

        public static async void PushPost(string accessToken, string caption, Uri[] imagesUrls)
        {
            var config = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = accessToken,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
            };

            var client = new DiscordClient(config);

            client.Ready += Client_Ready;

            await client.ConnectAsync();

            var channel = await client.GetChannelAsync(THREAD_ID);

            if (channel != null)
            {
                var embedBuilder = new DiscordEmbedBuilder();

                var message = new DiscordMessageBuilder();

                for (var i = 0; i < imagesUrls.Length; i++)
                {
                    message
                        .WithEmbed(embedBuilder
                            .WithImageUrl(imagesUrls[i])
                            .WithDescription(caption)
                            .Build());
                    await client.SendMessageAsync(channel, message);
                }
            }
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
