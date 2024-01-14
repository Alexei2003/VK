using Newtonsoft.Json;

namespace LikesRepostsBots.Classes
{
    internal sealed class PeoplesLIst
    {
        private HashSet<long> people = [];

        public void Read()
        {
            string json = File.ReadAllText(Path.Combine("PeopleDictionary.txt"));
            if (json?.Length != 0)
            {
                people = JsonConvert.DeserializeObject<HashSet<long>>(
                    json);
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
