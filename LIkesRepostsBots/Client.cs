using LikesRepostsBots.Classes;
using MyCustomClasses;
using static LikesRepostsBots.Classes.BotsWorksParams;

namespace LikesRepostsBots
{
    internal static class Client
    {
        public static void Start()
        {
            BotsWorksParams botParams = new();

            bool BanPeopleFromGroup = false;

            Console.WriteLine(
                "Выбор целевого действия:\n" +
                "1.Сделать репосты\n" +
                "2.Добавить в друзья\n" +
                "3.Очистка друзей\n" +
                "4.Добавить людей из группы в чс");

            string strMain = Console.ReadLine();

            if (strMain.Contains('1'))
            {
                botParams.MakeRepost = true;
            }
            if (strMain.Contains('2'))
            {
                Console.WriteLine("Сколько друзей добавить");
                botParams.AddFriendsCount = Convert.ToInt32(Console.ReadLine());
            }
            if (strMain.Contains('3'))
            {
                Console.WriteLine(
                    "Выбор способа очистки:\n" +
                    "1.Поверхностная (бан собачек)\n" +
                    "2.Глубокая (бан собачек и не соответствующих критериям) ");

                string strClear = Console.ReadLine();

                if (strClear.Contains('1'))
                {
                    botParams.ClearFriends = ClearFriendsType.BanAccount;
                }
                if (strClear.Contains('2'))
                {
                    botParams.ClearFriends = ClearFriendsType.BanAndMathAccount;
                }
            }
            if (strMain.Contains('4'))
            {
                BanPeopleFromGroup = true;

                Console.WriteLine("Id группы");
                botParams.GroupIdForBad = Convert.ToInt64(Console.ReadLine());
            }

            var accessTokensAndNames = File.ReadAllLines(Path.Combine("AccessTokens.txt"));

            var bots = new BotsList(accessTokensAndNames);

            botParams.GroupIdForGood = 220199532;
            for (int i = 0; i < bots.Count; i++)
            {
                if (BanPeopleFromGroup && RandomStatic.Rand.Next(3) == 0)
                {
                    botParams.BanPeopleFromGroup = true;
                }
                bots[i].Start(botParams);
                if (botParams.BanPeopleFromGroup)
                {
                    BanPeopleFromGroup = false;
                    botParams.BanPeopleFromGroup = false;
                }

                Thread.Sleep(10 * RandomStatic._1SECOND + RandomStatic.Rand.Next(20 * RandomStatic._1SECOND));
            }

            Console.WriteLine("\n------------------------------------------Финал------------------------------------------\n");

            Console.ReadLine();
        }
    }
}

