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

            PeoplesLIst people = new();
            people.Read();

            var bots = new BotsLIst(accessTokensAndNames, people, rand, false);

            const int TIME_WORK = 10 * 60 * 60 * 1000;
            int count = 0;
            while (true)
            {
                bots.Mix();

                int stepBetweenBots = TIME_WORK / bots.Count;

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
                    Console.WriteLine($"Бот номер {i + 1} {bots[i].BotName}\n" +
                                      $"Номер итерации {count}");
                    bots[i].Start(botParams);
                    Console.WriteLine($"Бот номер {i + 1} {bots[i].BotName}\n" +
                                      $"Номер итерации {count}");
                    Thread.Sleep(rand.Next(stepBetweenBots));
                }

                botParams.ClearFriends = ClearFriendsType.None;

                people.Write();

                count++;
            }
        }
    }
}
