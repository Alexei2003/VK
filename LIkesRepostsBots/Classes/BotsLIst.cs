using MyCustomClasses;

namespace LikesRepostsBots.Classes
{
    internal sealed class BotsList
    {
        private readonly List<SpamBot> bots = [];
        public int Count { get; private set; }

        public BotsList(string[] accessTokensAndNames)
        {
            for (int i = 1; i < accessTokensAndNames.Length; i += 2)
            {
                bots.Add(new SpamBot(accessTokensAndNames[i - 1], accessTokensAndNames[i]));
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
                int k = RandomStatic.Rand.Next(n--);
                (bots[n], bots[k]) = (bots[k], bots[n]);
            }
        }
    }
}
