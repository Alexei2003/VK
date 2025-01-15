using Newtonsoft.Json;

namespace AddDataInDataSet
{
    internal class TagsList
    {
        private HashSet<string>? tagsList = null;

        public TagsList()
        {
            Load();
        }

        public void Load()
        {
            try
            {
                string json = File.ReadAllText("TagsDictionary.txt");
                if (json?.Length != 0)
                {
                    tagsList = JsonConvert.DeserializeObject<HashSet<string>>(json);
                }
                else
                {
                    tagsList = new HashSet<string>();
                }
            }
            catch
            {
                tagsList = new HashSet<string>();
            }
        }

        public bool ContainTag(string tag)
        {
            if (tagsList.Contains(tag))
            {
                return true;
            }
            return false;
        }
    }
}
