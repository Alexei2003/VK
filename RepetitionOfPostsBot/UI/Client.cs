namespace RepetitionOfPostsBot.UI
{
    public class Client : BaseUI
    {
        public static void Start()
        {
            Initialize();

            Console.WriteLine("Количество постов");
            string countStr = Console.ReadLine();
            if (int.TryParse(countStr, out var count))
            {
                while (count > 0)
                {
                    Console.WriteLine($"Осталось: {count}");
                    _task?.RunAll(true).Wait();
                    count--;
                }
            }
        }
    }
}
