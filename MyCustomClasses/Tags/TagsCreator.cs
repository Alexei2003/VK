namespace MyCustomClasses.Tags
{
    public static class TagsCreator
    {
        public static string[] BASE_TAGS =
        [
            "#Anime",
            "#Arts",
            "#Art"
        ];

        public static string GetBasePartOfTag()
        {
            string tags = "";

            foreach (string tag in BASE_TAGS)
            {
                tags += tag;
            }

            tags += "\n";

            return tags;
        }

        public static string ReplaceTagToInstagram(string tag)
        {
            return tag.Replace("_", "");
        }
    }
}
