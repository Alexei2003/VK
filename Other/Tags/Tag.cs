namespace Other.Tags
{
    public class Tag
    {
        public string Name { get; set; } = "";
        public string Gelbooru { get; set; } = "";

        public Tag(string name, string gelbooru)
        {
            Name = name;
            Gelbooru = gelbooru;
        }
    }
}
