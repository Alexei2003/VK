using Other;

namespace RepetitionOfPostsBot.UI
{
    public class Client : BaseUI
    {
        public static void Start(bool auto = false)
        {
            Initialize();
            Gelbooru.UseProxy = false;
            string countStr;

            if (auto)
            {
                countStr = "24";
            }
            else
            {
                Console.WriteLine("Количество постов");
                countStr = Console.ReadLine();
            }

            if (int.TryParse(countStr, out var count))
            {
                while (count > 0)
                {
                    Console.WriteLine($"Осталось: {count}");
                    _task?.RunAll(true).Wait();
                    count--;
                }
            }

            Console.WriteLine("\n------------------------------------------Финал------------------------------------------\n");

            Console.ReadLine();
        }
    }
}
