using System.Collections.Immutable;
using System.Text;

namespace Other.Tags.Editors
{
    public static class BaseTagsEditor
    {
        private static readonly ImmutableArray<string> BASE_TAGS =
        [
            "Anime",
            "Arts",
            "Art"
        ];

        private static readonly ImmutableArray<string> BASE_TAGS_DELETE =
        [
            "Anime",
            "Arts",
            "Art",
            "Art\n"
        ];

        public static string GetBaseTags()
        {
            StringBuilder bld = new StringBuilder();
            foreach (string tag in BASE_TAGS)
            {
                bld.Append($"#{tag}");
            }

            return bld.ToString();
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
                for (int j = 0; j < BASE_TAGS_DELETE.Length; j++)
                {
                    if (sourceTags[i] == BASE_TAGS_DELETE[j])
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

            StringBuilder bld = new StringBuilder();
            for (int i = 0; i < sourceTags.Length; i++)
            {
                if (!listFindIndex.Contains(i))
                {
                    bld.Append($"#{sourceTags[i]}");
                }
            }

            return bld.ToString();
        }

        public static string FixTagString(string tagsStr)
        {
            // Удаление пробелов по краям
            tagsStr = tagsStr.Trim(' ');

            // Замена пробелов на _
            var tagsArr = tagsStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tagsArr.Length; i++)
            {
                tagsArr[i] = new string([.. tagsArr[i].Where(c => char.IsLetterOrDigit(c) || c == '#' || c == '_')]).ToLower();
            }
            tagsStr = string.Join("_", tagsArr);

            // Проверка первого # 
            if (tagsStr[0] != '#')
            {
                tagsStr = "#" + tagsStr;
            }

            return tagsStr;
        }

    }
}
