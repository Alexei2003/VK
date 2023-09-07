using LikesRepostsBots.Classes;

namespace HelloWorld
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Добавить друзей сколько раз ");
            int answer = Convert.ToInt32(Console.ReadLine());

            string groupId = "220199532";

            var accessTokens = File.ReadAllLines("AccessTokens.txt");

            {
                var comments = new CommentsDictionary();
                Console.WriteLine($"Количество комментариев в списке {comments.Count}");
            }

            var bots = new Bots(accessTokens);
            var rand = new Random();

            for (int j = 0; j < answer; j++)
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    bots[i].Start(groupId, answer);
                    Thread.Sleep(TimeSpan.FromSeconds(rand.Next(5) + 1));
                }
                Console.WriteLine($"\n\nИтерация № {j + 1}\n\n");
            }
            Console.ReadLine();
        }
    }
}