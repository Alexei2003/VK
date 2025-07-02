namespace RepetitionOfPostsBot.UI
{
    public class Server : BaseUI
    {
        public static void Start()
        {
            Initialize();
            while (true)
            {
                _task?.RunAll();

                Thread.Sleep(TimeSpan.FromHours(1));
            }
        }
    }
}
