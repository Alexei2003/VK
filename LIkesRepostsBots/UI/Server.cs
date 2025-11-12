using LikesRepostsBots.Classes;

using Other;

using static LikesRepostsBots.Classes.BotsWorksParams;

namespace LikesRepostsBots.UI
{
    public class Server : BaseUI
    {
        public static void Start()
        {
            Initialize();
            PeoplesList.Load();

            BotsWorksParams botParams = new()
            {
                MakeRepost = true,
                AddFriendsCount = 1,
                GroupIdForGood = 220199532
            };

            const int TIME_WORK = 3 * RandomStatic._1HOUR;
            const int TIME_WORK_RANDOM = 3 * RandomStatic._1HOUR;
            int count = 0;
            int stepBetweenBots = TIME_WORK / _botList.Count;
            int stepBetweenBotsRandom = TIME_WORK_RANDOM / _botList.Count;
            var indexRip = new Stack<int>(_botList.Count);
            int addFriend = 1;
            while (true)
            {
                try
                {
                    if (_botList.Count == 0)
                    {
                        Initialize();
                    }

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

                    for (int i = 0; i < _botList.Count; i++)
                    {
                        if (!_botList[i].Start(botParams))
                        {
                            indexRip.Push(i);
                        }
                        Thread.Sleep(stepBetweenBots + RandomStatic.Rand.Next(stepBetweenBotsRandom));
                    }

                    if (indexRip.Count > 0)
                    {
                        for (var i = 0; i < indexRip.Count; i++)
                        {
                            _botList.Remove(indexRip.Pop());
                        }
                        stepBetweenBots = TIME_WORK / _botList.Count;
                        stepBetweenBotsRandom = TIME_WORK_RANDOM / _botList.Count;
                        indexRip.Clear();
                    }

                    botParams.ClearFriends = ClearFriendsType.None;

                    PeoplesList.Save();

                    count++;
                }
                catch (Exception e)
                {
                    Logs.WriteException(e);
                    Thread.Sleep(10);
                    continue;
                }
            }
        }
    }
}
