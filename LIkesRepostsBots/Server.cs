using LikesRepostsBots.Classes;
using MyCustomClasses;
using static LikesRepostsBots.Classes.BotsWorksParams;

namespace LikesRepostsBots
{
    internal static class Server
    {
        public static void Start()
        {
            var thread = new Thread(new ParameterizedThreadStart(StartBot));
            thread.Start();

            while (true)
            {
                Console.WriteLine("LikesRepostsBots work " + DateTime.Now.ToString());
                Thread.Sleep(TimeSpan.FromHours(1));
            }
        }

        public static void StartBot(object data)
        {
            BotsWorksParams botParams = new()
            {
                MakeRepost = true,
                AddFriendsCount = 1,
                GroupIdForGood = 220199532
            };

            var accessTokensAndNames = File.ReadAllLines(Path.Combine("AccessTokens.txt"));

            PeoplesLIst people = new();
            people.Read();

            var bots = new BotsList(accessTokensAndNames, people);


            const int TIME_WORK = 3 * RandomStatic._1HOUR;
            const int TIME_WORK_RANDOM = 3 * RandomStatic._1HOUR;
            int count = 0;
            int stepBetweenBots = TIME_WORK / bots.Count;
            int stepBetweenBotsRandom = TIME_WORK_RANDOM / bots.Count;
            var indexRip = new Stack<int>(bots.Count);
            int addFriend = 1;
            while (true)
            {
                try
                {
                    if (bots.Count == 0)
                    {
                        bots = new BotsList(accessTokensAndNames, people);
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

                    if (addFriend <= 0)
                    {
                        addFriend = 4;
                        botParams.AddFriendsCount = RandomStatic.Rand.Next(4);
                    }
                    else
                    {
                        addFriend--;
                        botParams.AddFriendsCount = 0;
                    }

                    for (int i = 0; i < bots.Count; i++)
                    {
                        if (!bots[i].Start(botParams))
                        {
                            indexRip.Push(i);
                        }
                        Thread.Sleep(stepBetweenBots + RandomStatic.Rand.Next(stepBetweenBotsRandom));
                    }

                    if (indexRip.Count > 0)
                    {
                        for (var i = 0; i < indexRip.Count; i++)
                        {
                            bots.Remove(indexRip.Pop());
                        }
                        stepBetweenBots = TIME_WORK / bots.Count;
                        stepBetweenBotsRandom = TIME_WORK_RANDOM / bots.Count;
                        indexRip.Clear();
                    }

                    botParams.ClearFriends = ClearFriendsType.None;

                    people.Write();

                    count++;
                }
                catch (Exception e)
                {
                    Logs.WriteExcemption(e);
                    Thread.Sleep(10);
                    continue;
                }
            }
        }
    }
}
