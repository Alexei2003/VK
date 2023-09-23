using LikesRepostsBots.Classes;

namespace HelloWorld
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Добавить друзей сколько раз ");
            int numbPeople = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Очистка");
            string clearFriends = Console.ReadLine();

            string groupId = "220199532";

            var accessTokens = File.ReadAllLines("AccessTokens.txt");

            PeopleDictionary people = new();

            var rand = new Random();
            var bots = new Bots(accessTokens, people, rand);

            people.Read();

            for (int j = 0; j <= numbPeople; j++)
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    bots[i].Start(groupId, j, clearFriends);
                    Thread.Sleep(TimeSpan.FromSeconds(rand.Next(5) + 1));
                }
                Console.WriteLine($"\n\nИтерация № {j + 1}\n\n");
            }
            people.Write();

            Console.ReadLine();
        }
    }
}