using Newtonsoft.Json;

namespace LikesRepostsBots.Classes
{
    internal class CommentsDictionary
    {
        private readonly string[] comments;
        public int Count { get; }

        public CommentsDictionary()
        {
            string json = File.ReadAllText("CommentsDictionary.txt");
            comments = JsonConvert.DeserializeObject<string[]>(json);
            Count = comments.Length;
        }

        public string this[int i]
        {
            get
            {
                return comments[i];
            }
        }
    }
}
