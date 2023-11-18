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

            var accessTokensAndNames = File.ReadAllLines("AccessTokens.txt");
            var rand = new Random();

            PeopleDictionary people = new();

            int count = 0;
            while (true)
            {
                var bots = new Bots(accessTokensAndNames, people, rand, false);

                people.Read();

                for (int i = 0; i < bots.Count; i++)
                {
                    bots[i].Start(botParams);
                }

                people.Write();

                count++;

                if (count >= 10)
                {
                    var BotsWorksParamsClear = new BotsWorksParams()
                    {
                        ClearFriends = ClearFriendsType.BanAccount
                    };
                    for (int i = 0; i < bots.Count; i++)
                    {
                        bots[i].Start(BotsWorksParamsClear);
                    }
                }

                if (count >= 50)
                {
                    count = 0;
                    var BotsWorksParamsClear = new BotsWorksParams()
                    {
                        ClearFriends = ClearFriendsType.BanAndMathAccount
                    };
                    for (int i = 0; i < bots.Count; i++)
                    {
                        bots[i].Start(BotsWorksParamsClear);
                    }
                }
            }
        }
    }
}
