using System.Collections.ObjectModel;
using System.Text.Json;

namespace Other.Tags.Collections
{
    public static class TagLoader
    {
        public const string PathFile = "E:\\WPS\\CommonData\\Tags\\TagsDictionary.txt";

        public static Collection<Tag> Load()
        {
            var collection  = new Collection<Tag>();
            try
            {
                string json = File.ReadAllText(PathFile);
                if (json?.Length != 0)
                {
                    collection = JsonSerializer.Deserialize<Collection<Tag>>(json);
                }
            }
            catch { }

            return collection ?? [];
        }
    }
}
