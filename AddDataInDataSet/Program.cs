using System.Drawing;

namespace AddDataInDataSet
{
    internal class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.WriteLine("" +
                    "Выбор действия\n" +
                    "1.Переместить из New\n" +
                    "2.Генерация искуственных данных\n" +
                    "Выход (напишите exit)\n");

                var action = Console.ReadLine();

                var thWrite = new Thread(WriteCountMake);

                int[] count = [0];

                switch (action)
                {
                    case "1":
                        count[0] = 0;
                        thWrite.Start(count);
                        WorkWithDirectory.MoveDataFromNewToReady(count);
                        Thread.Sleep(100);
                        count[0] = -1;
                        thWrite.Join();
                        break;
                    case "2":
                        count[0] = 0;
                        thWrite.Start(count);
                        WorkWithDirectory.MoveDataToOutput(count);
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

        private static void WriteCountMake(object obj)
        {
            var count = (int[])obj;

            int intWrite;

            while (true)
            {
                intWrite = count[0];

                if(intWrite == -1)
                {
                    break;
                }

                // Перемещаем курсор в начало строки
                Console.SetCursorPosition(0, Console.CursorTop);

                // Заменяем текущую строку новым текстом
                Console.Write("Обработано тегов : " + intWrite);
            }
        }
    }
}