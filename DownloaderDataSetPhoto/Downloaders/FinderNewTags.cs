using System.Text;

using HtmlAgilityPack;

using Other;
using Other.Tags.Collections;

namespace DownloaderDataSetPhoto.Downloaders
{
    public static class FinderNewTags
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string _fileName = "dump.txt";
        public static void CreateDumpFromGelbooru(TagList tagList)
        {
            var sw = File.CreateText(_fileName);
            const string url = "https://gelbooru.com/index.php?page=post&s=list&tags=";

            try
            {
                for (var i = 0; i < 10; i++)
                {
                    var htmlDocument = Gelbooru.GetPageHTML(_httpClient, url, i);

                    var nodesArr = htmlDocument.DocumentNode
                        .SelectNodes("//a[@id and contains(@href, 'https') and contains(@href, 'gelbooru.com')]")
                        .ToArray();

                    OpenArtsPage(nodesArr, tagList, sw);
                }
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
            }
        }

        private static void OpenArtsPage(HtmlNode[] nodesArr, TagList tagList, StreamWriter sw)
        {
            foreach (var node in nodesArr)
            {
                var href = node.GetAttributeValue("href", string.Empty);

                href = href.Replace("amp;", "");

                var htmlDocument = Gelbooru.GetPageHTML(_httpClient, href);

                // % знаки
                var nodeCharactersTagsArr = htmlDocument.DocumentNode
                    .SelectNodes("//li[contains(@class, 'tag-type-character')]")
                    ?.ToArray();

                if (nodeCharactersTagsArr == null)
                {
                    continue;
                }

                var next = false;
                foreach (var tag in nodeCharactersTagsArr)
                {
                    if (int.TryParse(tag.SelectSingleNode(".//span[2]").InnerText, out var count) && count > 500)
                    {
                        var tagStr = tag.SelectSingleNode("(.//a)[2]").InnerText.Trim().Replace(' ', '_');
                        if (tagList.Find("", tagStr).Count > 0)
                        {
                            next = true;
                        }
                    }
                    else
                    {
                        next = true;
                    }
                }

                if (next)
                {
                    continue;
                }

                var nodeCopyrightTagsArr = htmlDocument.DocumentNode
                    .SelectNodes("//li[contains(@class, 'tag-type-copyright')]/a[@href]")
                    ?.ToArray();

                if (nodeCopyrightTagsArr == null)
                {
                    continue;
                }

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("\n---------------------------------------------------------------------------------");
                stringBuilder.AppendLine("\ncharacter");
                foreach (var tag in nodeCharactersTagsArr)
                {
                    stringBuilder.AppendLine(tag.SelectSingleNode("(.//a)[2]").InnerText.Trim().Replace(' ', '_'));
                }
                stringBuilder.AppendLine("\ncopyright");
                foreach (var tag in nodeCopyrightTagsArr)
                {
                    stringBuilder.AppendLine(tag.InnerText.Trim().Replace(' ', '_'));
                }
                sw.Write(stringBuilder.ToString());
            }
        }
    }
}
