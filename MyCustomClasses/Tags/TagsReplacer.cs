namespace MyCustomClasses.Tags
{
    public class TagsReplacer
    {
        public static string ReplaceTagRemoveExcessFromVk(string tag)
        {
            tag = RemoveFirstChirFromTag(tag);

            return RemoveDogGroupFromTag(tag);
        }

        public static string RemoveFirstChirFromTag(string tag)
        {
            tag = tag.Replace(".", "");
            tag = tag.Replace("!", "");

            return tag;
        }

        public static string RemoveDogGroupFromTag(string tag)
        {
            var tags = tag.Split('#', StringSplitOptions.RemoveEmptyEntries);

            tag = "";

            for (var i = 0; i < tags.Length; i++)
            {
                tag += '#' + tags[i].Split('@', StringSplitOptions.RemoveEmptyEntries).First() + '\n';
            }

            return tag;
        }
    }
}
