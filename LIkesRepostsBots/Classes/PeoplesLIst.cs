using Newtonsoft.Json;

namespace LikesRepostsBots.Classes
{
    internal sealed class PeoplesLIst
    {
        private HashSet<long> people = [];

        public void Read()
        {
            string filePath = Path.Combine("PeopleDictionary.txt");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                if (json?.Length != 0)
                {
                    people = JsonConvert.DeserializeObject<HashSet<long>>(json);
                }
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

        public bool Add(long id)
        {
            if (people.Add(id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
