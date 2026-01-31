using Other;

using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace RepetitionOfPostsBot.UI
{
    public class Client : BaseUI
    {
        public static void Start(bool auto = false)
        {
            Initialize();
            Gelbooru.UseProxy = false;
            string countStr;
            var countPost = 0;

            if (auto)
            {
                var wall = _task?.VkApi.Wall.Get(new WallGetParams
                {
                    OwnerId = -1 * _task?.VkGroupId,
                    Count = 1,
                    Filter = WallFilter.Postponed,
                });

                countPost = 24 + ((24 * 14) / (int)wall.TotalCount);
            }
            else
            {
                Console.WriteLine("Количество постов");
                countStr = Console.ReadLine();
                int.TryParse(countStr, out countPost);
            }

            while (countPost > 0)
            {
                Console.WriteLine($"Осталось: {countPost}");
                _task?.RunAll(true, countPost).Wait();
                countPost--;
            }

            Console.WriteLine("\n------------------------------------------Финал------------------------------------------\n");

            Console.ReadLine();
        }
    }
}
