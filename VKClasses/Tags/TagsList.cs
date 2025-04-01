using System.Collections.Concurrent;
using System.Text.Json;

namespace VKClasses.Tags
{
    public sealed class TagsList
    {
        private List<string>? tagsList = null;

        public TagsList()
        {
            Load();
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(tagsList);
            File.WriteAllText("TagsDictionary.txt", json);
        }

        public void Load()
        {
            try
            {
                string json = File.ReadAllText("TagsDictionary.txt");
                if (json?.Length != 0)
                {
                    tagsList = JsonSerializer.Deserialize<List<string>>(json);
                }
                else
                {
                    tagsList = [];
                }
            }
            catch
            {
                tagsList = [];
            }
        }

        public ConcurrentStack<string> FindLast(string tagsGet)
        {
            ConcurrentStack<string> stack;
            var tags = tagsGet.Split('#');
            if (tags.Length > 0)
            {
                stack = Find(tags[^1]);
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
            LastTag = LastTag.ToLower();
            Parallel.ForEach(tagsList, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount }, tag =>
            {
                var lowerTag = tag.ToLower();
                if (lowerTag.Contains(LastTag))
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
