namespace MyCustomClasses.Tags
{
    public class TagsReplacer
    {
        public static string ReplaceTagRemoveExcessFromVk(string tag)
        {
            tag = tag.Replace(".", "");
            tag = tag.Replace("!", "");

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
