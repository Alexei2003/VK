using LikesRepostsBots.Classes;
using static LikesRepostsBots.Classes.BotsWorksParams;

namespace LikesRepostsBots
{
    internal static class Server
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

            var bots = new BotsList(accessTokensAndNames, people, rand);

            const int TIME_WORK = 24 * 60 * 60 * 1000;
            int count = 0;
            int stepBetweenBots = TIME_WORK / bots.Count;
            var indexRip = new Stack<int>(bots.Count);
            while (true)
            {
                if (bots.Count == 0)
                {
                    bots = new BotsList(accessTokensAndNames, people, rand);
                }

                bots.Mix();

                switch (count)
                {
                    case 7:
                        botParams.ClearFriends = ClearFriendsType.BanAccount;
                        break;
                    case 14:
                        count = 0;
                        botParams.ClearFriends = ClearFriendsType.BanAndMathAccount;
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < bots.Count; i++)
                {
                    if (!bots[i].Start(botParams))
                    {
                        indexRip.Push(i);
                    }
                    Thread.Sleep(rand.Next(stepBetweenBots));
                }

                if (indexRip.Count > 0)
                {
                    for (var i = 0; i < indexRip.Count; i++)
                    {
                        bots.Remove(indexRip.Pop());
                    }
                    stepBetweenBots = TIME_WORK / bots.Count;
                    indexRip.Clear();
                }

                botParams.ClearFriends = ClearFriendsType.None;

                people.Write();

                count++;
            }
        }
    }
}
