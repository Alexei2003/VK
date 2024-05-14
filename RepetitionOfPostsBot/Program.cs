using MyCustomClasses;
using MyCustomClasses.Instagram;
using MyCustomClasses.VK;
using Newtonsoft.Json;
using RepetitionOfPostsBot.BotsTask;

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

            var instagramApi = new InstagramApi(accessTokens.GetValueOrDefault(GosUslugi.INSTAGRAM), "100065029535432");
            var a = instagramApi.CreateMediaObjectAsync("https://scontent.cdninstagram.com/v/t51.29350-15/442706645_769424565341139_4575877684158403683_n.jpg?stp=dst-jpg_e15&efg=eyJ2ZW5jb2RlX3RhZyI6ImltYWdlX3VybGdlbi4xMDgweDEwODAuc2RyLmYyOTM1MCJ9&_nc_ht=scontent.cdninstagram.com&_nc_cat=103&_nc_ohc=VmIBLRDyJLcQ7kNvgHoYMv_&edm=APs17CUBAAAA&ccb=7-5&ig_cache_key=MzM2NzMzMDMxMzcyMDM2MDU5MQ%3D%3D.2-ccb7-5&oh=00_AYBxLce_MG8wa0oaM1DX4eqlKdWOBDb1JBx4qJoyJOf48A&oe=6647B38D&_nc_sid=10d13b", "тест");

            a.Wait();

            while (true)
            {
                Thread.Sleep(TimeSpan.FromDays(1000000));
            }
        }
    }
}