using System.Text.Json;

namespace LikesRepostsBots.Classes
{
    public static class PeoplesList
    {
        private const int CHUNK_SIZE = 1_000_000;
        private const string FILE_PREFIX = "E:\\\\WPS\\\\CommonData\\\\VK\\\\PeopleDictionary_";
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