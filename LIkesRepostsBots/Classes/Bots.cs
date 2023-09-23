namespace LikesRepostsBots.Classes
{
    internal class Bots
    {
        private readonly SpamBot[] bots;
        public int Count { get; }

        public Bots(string[] accessTokens, PeopleDictionary people, Random rand)
        {
            bots = new SpamBot[accessTokens.Length / 2];
            for (int i = 0; i < accessTokens.Length; i++)
            {
                if (i % 2 == 1)
                {
                    bots[i / 2] = new SpamBot(accessTokens[i], people, rand);
                }
            }
            Count = bots.Length;
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
