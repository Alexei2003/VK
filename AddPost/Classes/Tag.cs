using Newtonsoft.Json;

namespace AddPost.Classes
{
    internal class Tag
    {
        public List<string> TagsList { get; set; }

        public Tag()
        {
            TagsList = new List<string>();
        }

        public void SaveDictionary()
        {
            string json = JsonConvert.SerializeObject(TagsList);
            File.WriteAllText("TagsDictionary.txt", json);
        }

        public void LoadDictionary()
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

        public Stack<string> Find(string LastTag)
        {
            Stack<string> stack = new();
            var tags = LastTag.Split("#");
            if (tags.Length > 0)
            {
                LastTag = tags.Last();
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

        public bool Add(string tags)
        {
            if (!TagsList.Contains(tags))
            {
                TagsList.Add(tags);
                return true;
            }
            return false;
        }
    }
}
