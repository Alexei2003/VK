internal class Program
{
    private const string LITTLE_PATH = "ARTS\\Little";
    private const string NEW_PATH = "ARTS\\New";
    private const string READY_PATH = "ARTS\\Ready";

    private const int MIN_COUNT_FILES = 100;

    private static void Main(string[] args)
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

    private static void DirectoryMove(string source, string dest)
    {
        if (Directory.Exists(dest))
        {
            var sourceInfo = new DirectoryInfo(source);
            foreach (var file in sourceInfo.GetFiles())
            {
                file.MoveTo(dest + "\\" + file.Name);
            }
            Directory.Delete(source, true);
        }
        else
        {
            Directory.Move(source, dest);
        }
    }

    private static void MoveDataFromNew()
    {
        var readDirectories = Directory.GetDirectories(READY_PATH);
        var newDirectories = Directory.GetDirectories(NEW_PATH);

        foreach (var newDirectory in newDirectories)
        {
            var directoryNameParts = newDirectory.Split("\\");
            bool DirectoryAdded = false;
            foreach (var readyDirectory in readDirectories)
            {
                if (readyDirectory.Contains(directoryNameParts.Last()))
                {
                    DirectoryMove(newDirectory, readyDirectory);
                    DirectoryAdded = true;
                    break;
                }
            }
            if (!DirectoryAdded)
            {
                DirectoryMove(newDirectory, LITTLE_PATH + "\\" + directoryNameParts.Last());
            }
        }
    }

    private static void MoveDataFromLittle()
    {
        var littleDirectories = Directory.GetDirectories(LITTLE_PATH);

        foreach(var littleDirectory in littleDirectories)
        {
            var sourceInfo = new DirectoryInfo(littleDirectory);
            int a = sourceInfo.GetFiles().Length;
            if (sourceInfo.GetFiles().Length >= MIN_COUNT_FILES)
            {
                var directoryNameParts = littleDirectory.Split("\\");
                DirectoryMove(littleDirectory, READY_PATH + "\\" + directoryNameParts.Last());
            }
        }
    }
}