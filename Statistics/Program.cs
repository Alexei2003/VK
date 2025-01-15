using ClosedXML.Excel;
using MyCustomClasses;
using MyCustomClasses.Tags;
using MyCustomClasses.Tags.Editors;
using MyCustomClasses.VK;
using VkNet.Enums.StringEnums;

namespace Statistics
{
    internal static class Program
    {
        private const long GROUP_ID = 220199532;
        private const int SHIFT_POSTS = 100;

        private static void Main()
        {
            var accessTokens = GosUslugi.GetAccessTokens();

            var api = new VkApiCustom(accessTokens[GosUslugi.VK]);

            var wall = api.Wall.Get(new VkNet.Model.WallGetParams
            {
                OwnerId = -1 * GROUP_ID,
                Count = 100,
                Filter = WallFilter.All
            });

            ulong maxOffset = wall.TotalCount;
            ulong offset = 0;

            var dictTag = new Dictionary<string, TagInfo>();

            while (offset <= maxOffset)
            {
                wall = api.Wall.Get(new VkNet.Model.WallGetParams
                {
                    OwnerId = -1 * GROUP_ID,
                    Count = 100,
                    Offset = offset,
                    Filter = WallFilter.All
                });

                offset += SHIFT_POSTS;

                foreach (var post in wall.WallPosts)
                {
                    var postText = post.Text;

                    postText = BaseTagsEditor.RemoveBaseTags(postText);

                    postText = postText.Replace("!", "");
                    postText = postText.Replace(".", "");

                    var tagsArr = postText.Split('#', StringSplitOptions.RemoveEmptyEntries);

                    if (tagsArr.Length > 2 || tagsArr.Length == 0 || postText.Contains(' '))
                    {
                        continue;
                    }

                    postText = TagsReplacer.RemoveGroupLinkFromTag(postText);
                    postText = postText.Replace("\n", "");

                    var viewsCount = post.Views.Count;
                    var likesCount = post.Likes.Count;
                    var commentsCount = post.Comments.Count;
                    var repostsCount = post.Reposts.Count;

                    TagInfo tagInfo;
                    // Сбор инфы настроить
                    try
                    {
                        tagInfo = dictTag[postText];

                        tagInfo.Count++;
                        tagInfo.ViewsCount += viewsCount;
                        tagInfo.LikesCount += likesCount;
                        tagInfo.CommentsCount += commentsCount;
                        tagInfo.RepostsCount += repostsCount;

                        dictTag[postText] = tagInfo;
                    }
                    catch
                    {
                        tagInfo = new TagInfo()
                        {
                            Count = 1,
                            ViewsCount = viewsCount,
                            LikesCount = likesCount,
                            CommentsCount = commentsCount,
                            RepostsCount = repostsCount,
                        };
                        dictTag.Add(postText, tagInfo);

                    }
                }
                Console.WriteLine(offset);
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");

                worksheet.Cell(1, 1).Value = "Название";
                worksheet.Cell(1, 2).Value = "Количество постов";
                worksheet.Cell(1, 3).Value = "Просмотры";
                worksheet.Cell(1, 4).Value = "Лайки";
                worksheet.Cell(1, 5).Value = "Комментарии";
                worksheet.Cell(1, 6).Value = "Репосты";
                worksheet.Cell(1, 7).Value = "Просмотры среднее";
                worksheet.Cell(1, 8).Value = "Лайки среднее";
                worksheet.Cell(1, 9).Value = "Комментарии среднее";
                worksheet.Cell(1, 10).Value = "Репосты среднее";
                worksheet.Cell(1, 11).Value = "Счёт";

                int i = 2;
                var tagInfoAverange = new TagInfo();
                foreach (var tag in dictTag)
                {
                    worksheet.Cell(i, 1).Value = tag.Key;
                    worksheet.Cell(i, 2).Value = tag.Value.Count;
                    tagInfoAverange.Count += tag.Value.Count;
                    worksheet.Cell(i, 3).Value = tag.Value.ViewsCount;
                    tagInfoAverange.ViewsCount += tag.Value.ViewsCount;
                    worksheet.Cell(i, 4).Value = tag.Value.LikesCount;
                    tagInfoAverange.LikesCount += tag.Value.LikesCount;
                    worksheet.Cell(i, 5).Value = tag.Value.CommentsCount;
                    tagInfoAverange.CommentsCount += tag.Value.CommentsCount;
                    worksheet.Cell(i, 6).Value = tag.Value.RepostsCount;
                    tagInfoAverange.RepostsCount += tag.Value.RepostsCount;
                    worksheet.Cell(i, 7).Value = tag.Value.ViewsCount / tag.Value.Count;
                    worksheet.Cell(i, 8).Value = tag.Value.LikesCount / tag.Value.Count;
                    worksheet.Cell(i, 9).Value = tag.Value.CommentsCount / tag.Value.Count;
                    worksheet.Cell(i, 10).Value = tag.Value.RepostsCount / tag.Value.Count;
                    worksheet.Cell(i, 11).Value = (int)(0.05 * tag.Value.ViewsCount + 0.4 * tag.Value.LikesCount + 1 * tag.Value.CommentsCount + 0.8 * tag.Value.RepostsCount) / tag.Value.Count;

                    i++;
                }

                worksheet.Cell(i, 1).Value = "Итог";
                worksheet.Cell(i, 2).Value = tagInfoAverange.Count;
                worksheet.Cell(i, 3).Value = tagInfoAverange.ViewsCount;
                worksheet.Cell(i, 4).Value = tagInfoAverange.LikesCount;
                worksheet.Cell(i, 5).Value = tagInfoAverange.CommentsCount;
                worksheet.Cell(i, 6).Value = tagInfoAverange.RepostsCount;
                worksheet.Cell(i, 7).Value = tagInfoAverange.ViewsCount / tagInfoAverange.Count;
                worksheet.Cell(i, 8).Value = tagInfoAverange.LikesCount / tagInfoAverange.Count;
                worksheet.Cell(i, 9).Value = tagInfoAverange.CommentsCount / tagInfoAverange.Count;
                worksheet.Cell(i, 10).Value = tagInfoAverange.RepostsCount / tagInfoAverange.Count;
                worksheet.Cell(i, 11).Value = (int)(0.05 * tagInfoAverange.ViewsCount + 0.4 * tagInfoAverange.LikesCount + 1 * tagInfoAverange.CommentsCount + 0.8 * tagInfoAverange.RepostsCount) / tagInfoAverange.Count;

                worksheet.Row(i).Style.Fill.BackgroundColor = XLColor.Yellow;

                workbook.SaveAs("tags.xlsx");
            }
        }

        private struct TagInfo
        {
            public int Count { get; set; }
            public int ViewsCount { get; set; }
            public int LikesCount { get; set; }
            public int CommentsCount { get; set; }
            public int RepostsCount { get; set; }
        }
    }
}