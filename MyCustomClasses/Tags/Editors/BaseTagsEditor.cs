namespace MyCustomClasses.Tags.Editors
{
    public static class BaseTagsEditor
    {
        public static string[] BASE_TAGS =
        [
            "Anime",
            "Arts",
            "Art"
        ];

        public static string GetBaseTags()
        {
            string tags = "";

            foreach (string tag in BASE_TAGS)
            {
                tags += '#' + tag;
            }

            return tags;
        }

        public static string GetBaseTagsWithNextLine()
        {

            return GetBaseTags() + "\n";
        }

        public static string RemoveBaseTags(string sourceTag)
        {
            var sourceTags = sourceTag.Split('#', StringSplitOptions.RemoveEmptyEntries);

            var listFindIndex = new List<int>();

            for (int i = 0; i < sourceTags.Length; i++)
            {
                for (int j = 0; j < BASE_TAGS.Length; j++)
                {
                    if (sourceTags[i] == BASE_TAGS[j])
                    {
                        listFindIndex.Add(i);
                        break;
                    }
                }
            }

            if (listFindIndex.Count == 0)
            {
                return sourceTag;
            }

            var tagWithRemove = "";
            for (int i = 0; i < sourceTags.Length; i++)
            {
                if (!listFindIndex.Contains(i))
                {
                    tagWithRemove += '#' + sourceTags[i];
                }
            }

            return tagWithRemove;
        }
    }
}
