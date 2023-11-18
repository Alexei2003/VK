using LikesRepostsBots.Classes;
using System.Runtime.CompilerServices;
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

            var accessTokensAndNames = File.ReadAllLines("AccessTokens.txt");
            var rand = new Random();

            PeopleDictionary people = new();

            const int TIME_WORK = 6 * 60 * 60 * 1000;
            int count = 0;
            while (true)
            {
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
                    Thread.Sleep(stepBetweenBots);
                }

                people.Write();

                count++;
            }         
        }
    }
}
