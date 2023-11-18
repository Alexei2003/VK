using LikesRepostsBots;

namespace HelloWorld
{
    class Program
    {
        private static void Main()
        {
            Console.WriteLine(
                "Тип \n" +
                "1.Клиент\n" +
                "2.Сервер");
            switch (Convert.ToInt32(Console.ReadLine()))
            {
                case 1:
                    Client.Start();
                    break;
                case 2:
                    Server.Start();
                    break;
                default:
                    break;
            }
        }
    }
}