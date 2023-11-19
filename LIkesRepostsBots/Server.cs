using LikesRepostsBots.Classes;
using static LikesRepostsBots.Classes.BotsWorksParams;

namespace LikesRepostsBots
{
    internal class Server
    {
        public static void Start()
        {
            BotsWorksParams botParams = new()
            {
                MakeRepost = true,
                AddFriendsCount = 1,
                GroupIdForGood = 220199532
            };

            var accessTokensAndNames = File.ReadAllLines(Path.Combine("AccessTokens.txt"));
            var rand = new Random();

            PeopleDictionary people = new();

            const int TIME_WORK = 10 * 60 * 60 * 1000;
            int count = 0;
            while (true)
            {
                Console.WriteLine($"Итерация {count}");

                var bots = new Bots(accessTokensAndNames, people, rand, false);

                bots.Mix();

                int stepBetweenBots = TIME_WORK / bots.Count;

                people.Read();

                if (count % 10 == 0)
                {
                    botParams.ClearFriends = ClearFriendsType.BanAccount;
                }

                if (count >= 50)
                {
                    count = 0;
                    botParams.ClearFriends = ClearFriendsType.BanAndMathAccount;
                }

                for (int i = 0; i < bots.Count; i++)
                {
                    bots[i].Start(botParams);
                    botParams.ClearFriends = ClearFriendsType.None;
                    Console.WriteLine($"Бот номер {i + 1} {bots[i].BotName}");
                    Thread.Sleep(rand.Next(stepBetweenBots));
                }

                people.Write();

                count++;
            }
        }
    }
}
