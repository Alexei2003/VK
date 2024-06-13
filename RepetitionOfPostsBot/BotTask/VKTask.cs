using MyCustomClasses.Tags.Editors;
using MyCustomClasses.VK;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace RepetitionOfPostsBot.BotTask
{
    internal static class VKTask
    {
        private const string groupVKShortUrl = "@anime_art_for_every_day";
        private const Int64 groupVKId = 220199532;

        private static Random rand = new();

        private static string[] tagsNotRepost = ["#Угадайка"];

        public static void RepetitionOfVKPosts(object data)
        {
            var api = data as VkApiCustom;

            var wall = api.Wall.Get(new WallGetParams
            {
                OwnerId = -1 * groupVKId,
                Count = 1,
                Filter = WallFilter.Postponed
            });

            ulong indexResendedPost = Convert.ToUInt64(rand.Next(Convert.ToInt32(wall.TotalCount)));
            while (true)
            {
                try
                {
                    // Получение первого отложеного поста
                    wall = api.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * groupVKId,
                        Count = 1,
                        Filter = WallFilter.Postponed
                    });

                    if (wall.WallPosts.Count < 1 || ((wall.WallPosts.First().Date.Value.Hour) != (DateTime.UtcNow.AddHours(1).Hour)))
                    {
                        // Получение самого первого поста
                        wall = api.Wall.Get(new WallGetParams
                        {
                            OwnerId = -1 * groupVKId,
                            Count = 1,
                            Filter = WallFilter.All
                        });
                        var totalCountPosts = wall.TotalCount;
                        var offsetIndexPost = totalCountPosts - indexResendedPost;

                        if (offsetIndexPost < 1000)
                        {
                            indexResendedPost = 0;
                            continue;
                        }

                        var firstPostData = wall.WallPosts[0].Date;

                        // Получение поста по id 
                        wall = api.Wall.Get(new WallGetParams
                        {
                            OwnerId = -1 * groupVKId,
                            Offset = offsetIndexPost,
                            Count = 1,
                            Filter = WallFilter.All
                        });

                        // Выход если поста несуществует
                        if (wall.WallPosts.Count == 0)
                        {
                            indexResendedPost++;
                            continue;
                        }

                        var post = wall.WallPosts.First();
                        var tagsString = post.Text;

                        // Проверка тега
                        tagsString = BaseTagsEditor.RemoveBaseTags(tagsString);

                        var tagsArr = tagsString.Split('#', StringSplitOptions.RemoveEmptyEntries);

                        if (tagsArr.Length > 2 || tagsString.Contains('.') || tagsString.Contains(' '))
                        {
                            indexResendedPost++;
                            continue;
                        }

                        foreach(var tag in tagsNotRepost)
                        {
                            if (tagsString.Contains(tag))
                            {
                                indexResendedPost++;
                                continue;
                            }
                        }

                        tagsString = string.Join("", tagsArr.Select(s => "#" + s + groupVKShortUrl + "\n"));

                        tagsString = BaseTagsEditor.GetBaseTagsWithNextLine() + tagsString;


                        var mediaAttachmentList = new List<MediaAttachment>();

                        foreach (var attachment in post.Attachments)
                        {
                            if (attachment.Type.Name == "Photo")
                            {
                                mediaAttachmentList.Add(new PhotoMy { OwnerId = attachment.Instance.OwnerId, Id = attachment.Instance.Id, AccessKey = attachment.Instance.AccessKey });
                            }
                        }

                        if(mediaAttachmentList.Count == 0)
                        {
                            indexResendedPost++;
                            continue;
                        }

                        // Повторый пост
                        api.Wall.Post(new WallPostParams()
                        {
                            OwnerId = -1 * groupVKId,
                            FromGroup = true,
                            Message = '.' + tagsString,
                            Attachments = mediaAttachmentList,
                            PublishDate = firstPostData.Value.AddHours(1),
                        });

                        indexResendedPost += Convert.ToUInt64(1 + rand.Next(Convert.ToInt32(totalCountPosts / 100)));
                    }
                    Thread.Sleep(TimeSpan.FromMinutes(30));
                }
                catch
                {
                    indexResendedPost++;
                    continue;
                }
            }
        }
    }
}
