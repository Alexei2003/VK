using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace AddPost.Classes
{
    internal sealed class TagsLIst
    {
        private List<string> tagsList;

        public TagsLIst()
        {
            tagsList = [];
        }

        public void SaveDictionary()
        {
            string json = JsonConvert.SerializeObject(tagsList);
            File.WriteAllText("TagsDictionary.txt", json);
        }

        public void LoadDictionary()
        {
            try
            {
                string json = File.ReadAllText("TagsDictionary.txt");
                if (json?.Length != 0)
                {
                    tagsList = JsonConvert.DeserializeObject<List<string>>(json);
                }
            }
            catch { }
        }

        public ConcurrentStack<string> FindLast(string tagsGet)
        {
            ConcurrentStack<string> stack;
            var tags = tagsGet.Split("#");
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
            ParallelOptions options = new()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };
            Parallel.ForEach(tagsList, options, tag =>
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
