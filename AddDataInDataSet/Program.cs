namespace AddDataInDataSet
{
    internal static class Program
    {
        private const int MIN_COUNT_FILES = 100;
        private static void Main()
        {
            while (true)
            {
                Console.WriteLine("" +
                    "Выбор действия\n" +
                    "1.Переместить из New\n" +
                    "2.Генерация исcкуственных данных\n" +
                    "Выход (напишите exit)\n");

                var action = Console.ReadLine();

                switch (action)
                {
                    case "1":
                        WorkWithDirectory.MoveDataFromNewToReady();
                        break;
                    case "2":
                        WorkWithDirectory.MoveDataToOutput();
                        break;
                    case "exit":
                        return;
                    default:
                        break;
                }
            }
        }
    }
}