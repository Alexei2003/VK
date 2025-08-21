using Other;

namespace RepetitionOfPostsBot.UI
{
    public class Server : BaseUI
    {
        public static void Start()
        {
            Initialize();
            Gelbooru.UseProxy = true;
            while (true)
            {
                _task?.RunAll();

                Thread.Sleep(TimeSpan.FromHours(1));
            }
        }
    }
}
