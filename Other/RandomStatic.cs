namespace Other
{
    public static class RandomStatic
    {
        public static Random Rand { get; set; } = new Random();
        public const int _1DAY = 24 * _1HOUR;
        public const int _1HOUR = 60 * _1MINUTE;
        public const int _1MINUTE = 60 * _1SECOND;
        public const int _1SECOND = 1000;

    }
}
