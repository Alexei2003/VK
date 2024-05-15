using MyCustomClasses;
using MyCustomClasses.Instagram;

namespace RepetitionOfPostsBot
{
    internal static class Program
    {

        private static void Main()
        {
            var accessTokens = GosUslugi.GetAccessTokens();

            /*            var vkApi = new VkApiCustom(accessTokens.GetValueOrDefault(GosUslugi.VK));

                        var threadRepetitionOfPosts = new Thread(new ParameterizedThreadStart(VKTask.RepetitionOfVKPosts));
                        threadRepetitionOfPosts.Start(vkApi);*/

            InstagramApi.Test();

            while (true)
            {
                Thread.Sleep(TimeSpan.FromDays(10));
            }
        }
    }
}