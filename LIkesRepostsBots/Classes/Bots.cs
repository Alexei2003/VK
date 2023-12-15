namespace LikesRepostsBots.Classes
{
    internal class Bots
    {
        private readonly List<SpamBot> bots = new();
        public int Count { get; }

        private readonly Random rand;

        public Bots(string[] accessTokensAndNames, PeopleDictionary people, Random rand, bool memorial)
        {
            this.rand = rand;
            for (int i = 1; i < accessTokensAndNames.Length; i += 2)
            {
                if (i % 2 == 1)
                {
                    bots.Add(new SpamBot(accessTokensAndNames[i - 1], accessTokensAndNames[i], people, rand));
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
