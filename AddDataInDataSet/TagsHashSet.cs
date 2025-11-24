using System.Text.Json;

using Other.Tags;

namespace AddDataInDataSet
{
    public class TagsHashSet
    {
        private const string PathFile = "E:\\WPS\\CommonData\\Tags\\TagsDictionary.txt";
        private HashSet<string> tagsHashSet = [];

        public TagsHashSet()
        {
            Load();
        }

        public void Load()
        {
            try
            {
                string json = File.ReadAllText(PathFile);
                if (json?.Length != 0)
                {
                    var list = JsonSerializer.Deserialize<List<Tag>>(json);
                    tagsHashSet = [.. list.Select(t => t.Name)];
                }
                else
                {
                    tagsHashSet = [];
                }
            }
            catch
            {
                tagsHashSet = [];
            }
        }

        public bool ContainTag(string tag)
        {
            if (tagsHashSet.Contains(tag))
            {
                return true;
            }
            return false;
        }
    }
}
