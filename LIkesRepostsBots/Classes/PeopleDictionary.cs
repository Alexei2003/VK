using Newtonsoft.Json;

namespace LikesRepostsBots.Classes
{
    internal class PeopleDictionary
    {
        private HashSet<long> people = new();

        public void Read()
        {
            string json = File.ReadAllText(Path.Combine("PeopleDictionary.txt"));
            if (json != "")
            {
                people = JsonConvert.DeserializeObject<HashSet<long>>(json);
            }
        }

        public void Write()
        {
            string json = JsonConvert.SerializeObject(people);
            File.WriteAllText(Path.Combine("PeopleDictionary.txt"), json);
        }

        public bool Contains(long id)
        {
            if (people.Contains(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Add(long id)
        {
            people.Add(id);
        }

        public void AddNotContains(long id)
        {
            if (!Contains(id))
            {
                Add(id);
            }
        }
    }
}
