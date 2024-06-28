using MyCustomClasses;
using MyCustomClasses.VK;
using Newtonsoft.Json.Linq;
using RepetitionOfPostsBot.BotTask;
using System.Net;
using System.Text;
using VkNet.Abstractions;
using VkNet.Model;
using VkNet.Utils;

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
                Thread.Sleep(TimeSpan.FromDays(10));
            }
        }
    }
}