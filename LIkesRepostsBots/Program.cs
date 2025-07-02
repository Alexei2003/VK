using LikesRepostsBots.UI;

namespace LikesRepostsBots
{
    static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
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
            else
            {
                switch (args[0])
                {
                    case "-client":
                        Client.Start();
                        break;
                    case "-server":
                        Server.Start();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}