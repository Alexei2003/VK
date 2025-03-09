using System.Text.Json;

namespace LikesRepostsBots.Classes
{
    internal static class PeoplesList
    {
        private const int CHUNK_SIZE = 1_000_000;
        private const string FILE_PREFIX = "PeopleDictionary_";
        private const string FILE_EXTENSION = ".txt";

        private static HashSet<long> _currentChunk = new();
        private static int _currentFileIndex = 0;

        private static void LoadChunk(int fileIndex)
        {
            _currentChunk.Clear();
            string filePath = FILE_PREFIX + fileIndex + FILE_EXTENSION;
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    _currentChunk = JsonSerializer.Deserialize<HashSet<long>>(json) ?? new HashSet<long>();
                }
            }
            _currentFileIndex = fileIndex;
        }

        private static void SaveChunk()
        {
            string json = JsonSerializer.Serialize(_currentChunk);
            File.WriteAllText(FILE_PREFIX + _currentFileIndex + FILE_EXTENSION, json);
        }

        public static bool Contains(long id)
        {
            int fileIndex = 0;
            while (true)
            {
                string filePath = FILE_PREFIX + fileIndex + FILE_EXTENSION;
                if (!File.Exists(filePath))
                    return false;

                LoadChunk(fileIndex);
                if (_currentChunk.Contains(id))
                    return true;

                fileIndex++;
            }
        }

        public static bool Add(long id)
        {
            int fileIndex = 0;
            while (true)
            {
                string filePath = FILE_PREFIX + fileIndex + FILE_EXTENSION;
                if (!File.Exists(filePath))
                {
                    _currentChunk.Clear();
                    _currentChunk.Add(id);
                    _currentFileIndex = fileIndex;
                    SaveChunk();
                    return true;
                }

                LoadChunk(fileIndex);
                if (_currentChunk.Contains(id))
                    return false;

                if (_currentChunk.Count < CHUNK_SIZE)
                {
                    _currentChunk.Add(id);
                    SaveChunk();
                    return true;
                }

                fileIndex++;
            }
        }
    }
}

class ConvertOldFile
{
    private const int CHUNK_SIZE = 1_000_000;
    private const string OLD_FILE = "PeopleDictionary.txt";
    private const string FILE_PREFIX = "PeopleDictionary_";
    private const string FILE_EXTENSION = ".txt";

    public static void Convert()
    {
        if (!File.Exists(OLD_FILE))
        {
            Console.WriteLine("Старый файл не найден.");
            return;
        }

        string json = File.ReadAllText(OLD_FILE);
        HashSet<long>? people = JsonSerializer.Deserialize<HashSet<long>>(json);
        if (people == null || people.Count == 0)
        {
            Console.WriteLine("Файл пуст или некорректен.");
            return;
        }

        var peopleArray = people.ToArray();
        int fileIndex = 0;

        for (int i = 0; i < peopleArray.Length; i += CHUNK_SIZE)
        {
            var chunk = peopleArray.Skip(i).Take(CHUNK_SIZE).ToHashSet();
            string newJson = JsonSerializer.Serialize(chunk);
            File.WriteAllText(FILE_PREFIX + fileIndex + FILE_EXTENSION, newJson);
            fileIndex++;
        }

        Console.WriteLine($"Файл успешно разбит на {fileIndex} частей.");
    }
}
