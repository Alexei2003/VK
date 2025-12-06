namespace Other
{
    public static class RandomStatic
    {
        public static Random Rand { get; set; } = new Random();
        public const int Day = 24 * Hour;
        public const int Hour = 60 * Minute;
        public const int Minute = 60 * Second;
        public const int Second = 1000;

    }
}
