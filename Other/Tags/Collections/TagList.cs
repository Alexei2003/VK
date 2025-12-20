using System.Collections.Concurrent;
using System.Text.Json;

namespace Other.Tags.Collections
{
    public class TagList
    {
        public List<Tag> Collection = [];
        private bool _changed = false;

        public TagList()
        {
            Collection = [.. TagLoader.Load()];
        }

        public void Save()
        {
            if (_changed)
            {
                const string PathFileOld = TagLoader.PathFile + ".old";
                File.Delete(PathFileOld);
                File.Move(TagLoader.PathFile, PathFileOld);
                string json = JsonSerializer.Serialize(Collection);
                File.WriteAllText(TagLoader.PathFile, json);
            }
        }

        public ConcurrentStack<Tag> FindLast(string tagGet, string gelbooruTag = "")
        {
            ConcurrentStack<Tag> stack;
            var tags = tagGet.Split('#');
            if (tags.Length > 0 || gelbooruTag.Length > 0)
            {
                stack = Find(tags[^1], gelbooruTag);
            }
            else
            {
                stack = new();
            }
            return stack;
        }

        public ConcurrentStack<Tag> Find(string lastTag, string gelbooruTag = "")
        {
            ConcurrentStack<Tag> stack = new();
            lastTag = lastTag.ToLower();
            Parallel.ForEach(Collection, tag =>
            {
                var lowerTag = tag.Name.ToLower();
                if ((gelbooruTag.Length == 0 && lowerTag.Contains(lastTag)) || (gelbooruTag.Length > 0 && lowerTag.Contains(gelbooruTag)))
                {
                    stack.Push(tag);
                }
            });
            return stack;
        }

        public void AddTagChangeGelbooru(Tag tag)
        {
            _changed = true;
            var findTag = Collection.FirstOrDefault(t => t.Name == tag.Name);
            if (findTag == null)
            {
                Collection.Add(tag);
            }
            else
            {
                findTag.Gelbooru = tag.Gelbooru;
            }
        }

        public bool Remove(string tag)
        {
            _changed = true;
            var tagClass = Collection.FirstOrDefault(t => t.Name == tag);
            if (tagClass != null)
            {
                return Collection.Remove(tagClass);
            }
            return true;
        }
    }
}
