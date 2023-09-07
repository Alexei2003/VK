using Newtonsoft.Json;
using VkNet;

namespace AddPost.Classes
{
    internal class Tag
    {
        public List<string> TagsList { get; set; }

        public Tag()
        {
            TagsList = new List<string>();
        }

        public void SaveTagsDictionary()
        {
            string json = JsonConvert.SerializeObject(TagsList);
            File.WriteAllText("TagsDictionary.txt", json);
        }

        public void LoadTagsDictionary()
        {
            try
            {
                string json = File.ReadAllText("TagsDictionary.txt");
                if (json != "")
                {
                    TagsList = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            catch { }
        }

        private static string? SeparateLastTag(string str)
        {
            int lastIndex = str.LastIndexOf('#');
            if (lastIndex != -1)
            {
                lastIndex++;
                return str[lastIndex..];
            }
            return null;
        }

        public Stack<string> FindTag(string LastTag)
        {
            Stack<string> stack = new();
            LastTag = SeparateLastTag(LastTag);
            if (LastTag != null)
            {
                LastTag = LastTag.ToUpper();
                ParallelOptions options = new()
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };
                Parallel.ForEach(TagsList, options, tag =>
                {
                    var upperTag = tag.ToUpper();
                    if (upperTag.Contains(LastTag))
                    {
                        stack.Push(tag);
                    }
                });
            }
            return stack;
        }

        public void AddTag(string tags)
        {
            if (!TagsList.Contains(tags))
            {
                TagsList.Add(tags);
            }
        }
    }
}
