using LikesRepostsBots.Classes;
using static LikesRepostsBots.Classes.BotsWorksParams;

namespace HelloWorld
{
    class Program
    {
        static void Main()
        {
            BotsWorksParams botParams = new BotsWorksParams();
            bool memorial = false;
            Console.WriteLine(
                "Выбор целевого действия:\n" +
                "1.Сделать репосты\n" +
                "2.Добавить в друзья\n" +
                "3.Очистка друзей\n" +
                "4.Мемориал\n" +
                "5.Добавить людей из группы в чс");

            string strMain = Console.ReadLine();

            if (strMain.Contains("1"))
            {
                botParams.MakeRepost = true;
            }
            if (strMain.Contains("2"))
            {
                Console.WriteLine("Сколько друзей добавить");
                botParams.AddFriendsCount = Convert.ToInt32(Console.ReadLine());
            }
            if (strMain.Contains("3"))
            {
                Console.WriteLine(
                    "Выбор способа очистки:\n" +
                    "1.Поверхностная (бан собачек)\n" +
                    "2.Глубокая (бан собачек и не соответствующих критериям) ");

                string strClear = Console.ReadLine();

                if (strClear.Contains("1"))
                {
                    botParams.ClearFriends = ClearFriendsType.BanAccount;
                }
                if (strClear.Contains("2"))
                {
                    botParams.ClearFriends = ClearFriendsType.BanAndMathAccount;
                }
            }
            if (strMain.Contains("4"))
            {
                memorial = true;
            }
            if (strMain.Contains("5"))
            {
                botParams.AddPeopleFromGroupInBlacklist = true;

                Console.WriteLine();
            }

            var accessTokensAndNames = File.ReadAllLines("AccessTokens.txt");

            PeopleDictionary people = new();

            var rand = new Random();
            var bots = new Bots(accessTokensAndNames, people, rand, memorial);

            people.Read();

            string groupId = "220199532";
            for (int i = 0; i < bots.Count; i++)
            {
                bots[i].Start(groupId, botParams);
                Thread.Sleep(TimeSpan.FromSeconds(rand.Next(5) + 1));
            }

            people.Write();

            Console.WriteLine("\n------------------------------------------Финал------------------------------------------\n");

            Console.ReadLine();
        }
    }
}