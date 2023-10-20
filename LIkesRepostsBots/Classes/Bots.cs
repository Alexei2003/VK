namespace LikesRepostsBots.Classes
{
    internal class Bots
    {
        private readonly List<SpamBot> bots;
        public int Count { get; }

        public Bots(string[] accessTokens, PeopleDictionary people, Random rand)
        {
            for (int i = 0; i < accessTokens.Length; i++)
            {
                if (i % 2 == 1)
                {
                    try
                    {
                        bots.Add(new SpamBot(accessTokens[i], people, rand));
                    }
                    catch
                    {
                        Console.WriteLine($"Бот {i/2+1} умер");
                    }
                }
            }
            Count = bots.Count;
        }

        public SpamBot this[int i]
        {
            get
            {
                Console.WriteLine($"Бот номер {i + 1}");
                return bots[i];
            }
        }
    }
}
