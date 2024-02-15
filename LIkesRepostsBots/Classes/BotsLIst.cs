namespace LikesRepostsBots.Classes
{
    internal sealed class BotsLIst
    {
        private readonly List<SpamBot> bots = [];
        public int Count { get; private set; }

        private readonly Random rand;

        public BotsLIst(string[] accessTokensAndNames, PeoplesLIst people, Random rand)
        {
            this.rand = rand;
            for (int i = 1; i < accessTokensAndNames.Length; i += 2)
            {
                bots.Add(new SpamBot(accessTokensAndNames[i - 1], accessTokensAndNames[i], people, rand));
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

        public void Remove(int i)
        {
            bots.RemoveAt(i);
            Count = bots.Count;
        }

        public void Mix()
        {
            int n = bots.Count;
            while (n > 1)
            {
                int k = rand.Next(n--);
                (bots[n], bots[k]) = (bots[k], bots[n]);
            }
        }
    }
}
