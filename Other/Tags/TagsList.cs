using System.Collections.Concurrent;
using System.Text.Json;

namespace Other.Tags
{
    public sealed class TagsList
    {
        public List<Tag> List { get; set; } = [];

        public TagsList()
        {
            Load();
        }

        public void Save()
        {
            string json = JsonSerializer.Serialize(List);
            File.WriteAllText("TagsDictionary.txt", json);
        }

        public void Load()
        {
            try
            {
                string json = File.ReadAllText("TagsDictionary.txt");
                if (json?.Length != 0)
                {
                    List = JsonSerializer.Deserialize<List<Tag>>(json);
                }
            }
            catch
            {
                List = [];
            }
        }

        public ConcurrentStack<Tag> FindLast(string tagsGet)
        {
            ConcurrentStack<Tag> stack;
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

        public ConcurrentStack<Tag> Find(string lastTag)
        {
            ConcurrentStack<Tag> stack = new();
            lastTag = lastTag.ToLower();
            Parallel.ForEach(List, tag =>
            {
                var lowerTag = tag.Name.ToLower();
                if (lowerTag.Contains(lastTag))
                {
                    stack.Push(tag);
                }
            });
            return stack;
        }

        public void AddTagChangeGelbooru(Tag tag)
        {
            var findTag = List.FirstOrDefault(t => t.Name == tag.Name);
            if (findTag == null)
            {
                List.Add(tag);
            }
            else
            {
                findTag.Gelbooru = tag.Gelbooru;
            }
        }

        public bool Remove(string tag)
        {
            var tagClass = List.FirstOrDefault(t => t.Name == tag);
            if (tagClass != null)
            {
                return List.Remove(tagClass);
            }
            return true;
        }
    }
}
