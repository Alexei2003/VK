using LikesRepostsBots.Classes;

namespace HelloWorld
{
    class Program
    {
        static void Main()
        {
            int makeRepost = 0;
            int addFriends = 0;
            int clearFriends = 0;
            int memorial = 0;

            Console.WriteLine(
                "Выбор целевого действия (если несколько писать через пробел числа):\n" +
                "1.Сделать репосты\n" +
                "2.Добавить в друзья\n" +
                "3.Очистка друзей\n" +
                "4.Мемориал");

            string strMain = Console.ReadLine();

            if (strMain.Contains("1"))
            {
                makeRepost = 1;
            }
            if (strMain.Contains("2"))
            {
                Console.WriteLine("Сколько друзей добавить");
                addFriends = Convert.ToInt32(Console.ReadLine());
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
                    clearFriends = 1;
                }
                if (strClear.Contains("2"))
                {
                    clearFriends = 2;
                }
            }
            if (strMain.Contains("4"))
            {
                memorial = 1;
            }

            var accessTokensAndNames = File.ReadAllLines("AccessTokens.txt");

            PeopleDictionary people = new();

            var rand = new Random();
            var bots = new Bots(accessTokensAndNames, people, rand);

            if (memorial != 1)
            {
                people.Read();

                string groupId = "220199532";
                for (int i = 0; i < bots.Count; i++)
                {
                    bots[i].Start(groupId, makeRepost, addFriends, clearFriends);
                    Thread.Sleep(TimeSpan.FromSeconds(rand.Next(5) + 1));
                }

                people.Write();
            }

            Console.ReadLine();
        }
    }
}