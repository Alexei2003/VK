using System.Text.Json;

namespace LikesRepostsBots.Classes
{
    public static class PeoplesList
    {
        private const string PathFile = "E:\\WPS\\CommonData\\VK\\PeopleDictionary.txt";

        private static HashSet<long> _peopleSet = new();

        public static void Load()
        {
            if (File.Exists(PathFile))
            {
                string json = File.ReadAllText(PathFile);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    _peopleSet = JsonSerializer.Deserialize<HashSet<long>>(json) ?? new HashSet<long>();
                }
            }
        }

        public static void Save()
        {
            File.Move(PathFile, PathFile + ".old");
            string json = JsonSerializer.Serialize(_peopleSet);
            File.WriteAllText(PathFile, json);
        }

        public static bool Add(long id)
        {
            return _peopleSet.Add(id);
        }
    }
}