using System.Collections.Concurrent;
using System.Text.Json;

namespace Other.Tags
{
    public sealed class TagsList
    {
        private List<string> tagsList = [];

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

        public ConcurrentStack<string> Find(string lastTag)
        {
            ConcurrentStack<string> stack = new();
            lastTag = lastTag.ToLower();
            Parallel.ForEach(tagsList, tag =>
            {
                var lowerTag = tag.ToLower();
                if (lowerTag.Contains(lastTag))
                {
                    stack.Push(tag);
                }
            });
            return stack;
        }

        public bool Add(string tag)
        {
            if (!tagsList.Contains(tag))
            {
                tagsList.Add(tag);
                return true;
            }
            return false;
        }

        public bool Remove(string tag)
        {
            return tagsList.Remove(tag);
        }
    }
}
