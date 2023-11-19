namespace LikesRepostsBots.Classes
{
    internal class Bots
    {
        private readonly List<SpamBot> bots = new();
        public int Count { get; }

        private Random rand;

        public Bots(string[] accessTokensAndNames, PeopleDictionary people, Random rand, bool memorial)
        {
            this.rand = rand;
            for (int i = 1; i < accessTokensAndNames.Length; i += 2)
            {
                if (i % 2 == 1)
                {
                    try
                    {
                        bots.Add(new SpamBot(accessTokensAndNames[i - 1], accessTokensAndNames[i], people, rand));

                    }
                    catch (Exception e) when (e is VkNet.Exception.UserAuthorizationFailException || e is VkNet.Exception.VkApiException)
                    {
                        if (memorial)
                        {
                            Console.WriteLine($"Бот {i / 2 + 1} умер. Вечная память {accessTokensAndNames[i - 1]}");
                        }
                    }
                }
            }
            Count = bots.Count;
        }

        public SpamBot this[int i]
        {
            get
            {
                return bots[i];
            }
        }

        public void Mix()
        {
            int n = bots.Count;
            while (n > 1)
            {
                int k = rand.Next(n--);
                var value = bots[k];
                bots[k] = bots[n];
                bots[n] = value;
            }
        }
    }
}
