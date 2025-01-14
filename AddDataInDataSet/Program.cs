namespace AddDataInDataSet
{
    internal static class Program
    {
        private static int[] count = [1];

        private static void Main()
        {
            while (true)
            {
                Console.WriteLine("" +
                    "Выбор действия\n" +
                    "1.Переместить из New\n" +
                    "2.Исправление Original\n" +
                    "3.Получить точность классов\n" +
                    "Выход (напишите exit)\n");

                var action = Console.ReadLine();

                var thWrite = new Thread(WriteCountMake);

                switch (action)
                {
                    case "1":
                        count[0] = 0;
                        thWrite.Start();
                        WorkWithDirectory.MoveDataFromNewToOriginal(count);
                        Thread.Sleep(100);
                        count[0] = -1;
                        thWrite.Join();
                        break;
                    case "2":
                        count[0] = 0;
                        thWrite.Start();
                        WorkWithDirectory.FixDataInOriginal(count);
                        Thread.Sleep(100);
                        count[0] = -1;
                        thWrite.Join();
                        break;
                    case "3":
                        count[0] = 0;
                        thWrite.Start();
                        WorkWithDirectory.GetAccuracyClassesOriginal(count);
                        Thread.Sleep(100);
                        count[0] = -1;
                        thWrite.Join();
                        break;
                    case "exit":
                        return;
                    default:
                        break;
                }
                Console.WriteLine();
            }
        }

        private static void WriteCountMake()
        {
            int intWrite;

            while (true)
            {
                intWrite = count[0];

                if (intWrite == -1)
                {
                    break;
                }

                // Перемещаем курсор в начало строки
                Console.SetCursorPosition(0, Console.CursorTop);

                // Заменяем текущую строку новым текстом
                Console.Write("Обработано тегов : " + intWrite);

                Thread.Sleep(1000);
            }
        }
    }
}