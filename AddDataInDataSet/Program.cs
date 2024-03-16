using DataSet;
using System.Drawing;

namespace AddDataInDataSet
{
    internal static class Program
    {
        private const string LITTLE_PATH = "ARTS\\Little";
        private const string NEW_PATH = "ARTS\\New";
        private const string READY_PATH = "ARTS\\Ready";

        private const int MIN_COUNT_FILES = 100;
        private static void Main()
        {
            while (true)
            {
                Console.WriteLine("" +
                    "Выбор действия\n" +
                    "1.Переместить из New\n" +
                   $"2.Переместить из Little где больше {MIN_COUNT_FILES}\n" +
                    "3.Выход\n");

                var action = Console.ReadLine();

                switch (action)
                {
                    case "1":
                        MoveDataFromNew();
                        break;
                    case "2":
                        MoveDataFromLittle();
                        break;
                    case "3":
                        return;
                    default:
                        break;
                }
            }
        }


        private static void DirectoryMove(string source, string destination)
        {
            if (Directory.Exists(destination))
            {
                var sourceInfo = new DirectoryInfo(source);
                var destinationInfo = new DirectoryInfo(destination);

                foreach (var src in sourceInfo.GetFiles())
                {
                    var similar = false;
                    using (var srcBmp = new Bitmap(src.FullName))
                    {
                        foreach (var dest in destinationInfo.GetFiles())
                        {
                            using var destBmp = new Bitmap(dest.FullName);
                            if (DataSetPhoto.IsSimilarPhoto(srcBmp, destBmp))
                            {
                                similar = true;
                                break;
                            }
                        }
                    }
                    if (!similar)
                    {
                        src.MoveTo(destination + "\\" + src.Name);
                    }
                }
                Directory.Delete(source, true);
            }
            else
            {
                Directory.Move(source, destination);
            }
        }

        private static void MoveDataFromNew()
        {
            int countReady = 0;
            int countLittle = 0;
            var readDirectories = Directory.GetDirectories(READY_PATH);
            var newDirectories = Directory.GetDirectories(NEW_PATH);

            var lockObj = new Object();

            Parallel.ForEach(newDirectories, newDirectory =>
            {
                var directoryNameParts = newDirectory.Split("\\", StringSplitOptions.RemoveEmptyEntries);
                bool DirectoryAdded = false;
                foreach (var readyDirectory in readDirectories)
                {
                    if (readyDirectory.Contains(directoryNameParts.Last()))
                    {
                        DirectoryMove(newDirectory, readyDirectory);
                        lock (lockObj)
                        {
                            countReady++;
                            DirectoryAdded = true;
                        }
                        break;
                    }
                }

                if (!DirectoryAdded)
                {
                    DirectoryMove(newDirectory, LITTLE_PATH + "\\" + directoryNameParts.Last());
                    countLittle++;
                }
            });

            Console.WriteLine($"Количество перенесеных в Little = {countLittle}");
            Console.WriteLine($"Количество перенесеных в Ready = {countReady}");
        }

        private static void MoveDataFromLittle()
        {
            int countReady = 0;
            var littleDirectories = Directory.GetDirectories(LITTLE_PATH);

            var lockObj = new Object();

            Parallel.ForEach(littleDirectories, littleDirectory =>
            {
                var sourceInfo = new DirectoryInfo(littleDirectory);
                if (sourceInfo.GetFiles().Length >= MIN_COUNT_FILES)
                {
                    var directoryNameParts = littleDirectory.Split("\\", StringSplitOptions.RemoveEmptyEntries);
                    DirectoryMove(littleDirectory, READY_PATH + "\\" + directoryNameParts.Last());
                    lock (lockObj)
                    {
                        countReady++;
                    }
                }
            });

            Console.WriteLine($"Количество перенесеных в Ready = {countReady}");
        }
    }
}