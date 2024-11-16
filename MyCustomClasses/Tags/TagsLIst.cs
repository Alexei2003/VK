using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace MyCustomClasses.Tags
{
    public sealed class TagsList
    {
        private List<string> tagsList = null;

        public TagsList()
        {
            Load();
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(tagsList);
            File.WriteAllText("TagsDictionary.txt", json);
        }

        public void Load()
        {
            try
            {
                string json = File.ReadAllText("TagsDictionary.txt");
                if (json?.Length != 0)
                {
                    tagsList = JsonConvert.DeserializeObject<List<string>>(json);
                }
                else
                {
                    tagsList = new List<string>();
                }
            }
            catch
            {
                tagsList = new List<string>();
            }
        }

        public ConcurrentStack<string> FindLast(string tagsGet)
        {
            ConcurrentStack<string> stack;
            var tags = tagsGet.Split('#');
            if (tags.Length > 0)
            {
                stack = Find(tags.Last());
            }
            else
            {
                stack = new();
            }
            return stack;
        }

        public ConcurrentStack<string> Find(string LastTag)
        {
            ConcurrentStack<string> stack = new();
            LastTag = LastTag.ToUpper();
            Parallel.ForEach(tagsList, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, tag =>
            {
                var upperTag = tag.ToUpper();
                if (upperTag.Contains(LastTag))
                {
                    stack.Push(tag);
                }
            });
            return stack;
        }

        public bool Add(string tags)
        {
            if (!tagsList.Contains(tags))
            {
                tagsList.Add(tags);
                return true;
            }
            return false;
        }

        public bool Remove(string tags)
        {
            return tagsList.Remove(tags);
        }
    }
}
