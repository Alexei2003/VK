namespace MyCustomClasses.Tags
{
    public class TagsReplacer
    {
        public static string ReplaceTagRemoveExcessFromVk(string tag)
        {
            tag = RemoveKeyCharFromTag(tag);

            return RemoveGroupLinkFromTag(tag);
        }

        public static string RemoveKeyCharFromTag(string tag)
        {
            tag = tag.Replace(".", "");
            tag = tag.Replace("!", "");
            

            return tag;
        }

        private static string TAG_REMOVE = "[club220199532|";

        public static string RemoveGroupLinkFromTag(string tag)
        {
            var tags = tag.Split('#', StringSplitOptions.RemoveEmptyEntries);

            tag = "";

            for (var i = 0; i < tags.Length; i++)
            {
                tag += '#' + tags[i].Split('@', StringSplitOptions.RemoveEmptyEntries).First() + '\n';
            }

            return tag.Replace(TAG_REMOVE,"");
        }
    }
}
