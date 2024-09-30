using MyCustomClasses;
using MyCustomClasses.Tags;
using MyCustomClasses.Tags.Editors;
using MyCustomClasses.VK;
using System.Net;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace RepetitionOfPostsBot.BotTask
{
    internal static class VKTask
    {
        private const string GROUP_SHORT_URL = "@anime_art_for_every_day";
        private const long GROUP_ID = 220199532;

        private static Random rand = new();

        private static string[] tagsNotRepost = ["Угадайка"];

        public static ulong GetRandomID(ulong max)
        {
            return Convert.ToUInt64(1 + rand.NextInt64(Convert.ToInt64(max)));
        }

        public static void RepeatVKPosts(object data)
        {
            var api = new VkApiCustom((string)data);

            // Получение первого отложеного поста
            var wall = api.Wall.Get(new WallGetParams
            {
                OwnerId = -1 * GROUP_ID,
                Count = 1,
                Filter = WallFilter.Postponed
            });

            ulong indexResendedPost = GetRandomID(wall.TotalCount);

            while (true)
            {
                try
                {
                    // Получение первого отложеного поста
                    wall = api.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * GROUP_ID,
                        Count = 1,
                        Filter = WallFilter.Postponed
                    });

                    if (wall.WallPosts.Count < 1 || ((wall.WallPosts.First().Date.Value.Hour) != (DateTime.UtcNow.AddHours(1).Hour)))
                    {
                        // Получение самого свежего поста
                        wall = api.Wall.Get(new WallGetParams
                        {
                            OwnerId = -1 * GROUP_ID,
                            Count = 2,
                            Filter = WallFilter.All
                        });

                        Post post;
                        if (wall.WallPosts[0].IsPinned.Value)
                        {
                            post = wall.WallPosts[1];
                        }
                        else
                        {
                            post = wall.WallPosts[0];
                        }


                        var totalCountPosts = wall.TotalCount;
                        var offsetIndexPost = totalCountPosts - indexResendedPost;
                        var notResendedCountPosts = totalCountPosts / 15;
                        var maxRandomOffsetRessendedPosts = totalCountPosts / 5;

                        if (offsetIndexPost < notResendedCountPosts)
                        {
                            indexResendedPost = offsetIndexPost;
                            continue;
                        }

                        var offsetNextPost = GetRandomID(maxRandomOffsetRessendedPosts);

                        var firstPostData = post.Date;

                        // Получение поста по id 
                        wall = api.Wall.Get(new WallGetParams
                        {
                            OwnerId = -1 * GROUP_ID,
                            Offset = offsetIndexPost,
                            Count = 1,
                            Filter = WallFilter.All
                        });

                        // Выход если поста несуществует
                        if (wall.WallPosts.Count == 0)
                        {
                            indexResendedPost += offsetNextPost;
                            continue;
                        }

                        post = wall.WallPosts.First();
                        var postText = post.Text;

                        // Проверка тега
                        postText = BaseTagsEditor.RemoveBaseTags(postText);
                        postText = TagsReplacer.RemoveGroupLinkFromTag(postText);
                        postText = postText.Replace("\n", "");

                        var tagsArr = postText.Split('#', StringSplitOptions.RemoveEmptyEntries);

                        if (tagsArr.Length > 2 || tagsArr.Length == 0 || postText.Contains('.') || postText.Contains(' ') || postText.Contains('!'))
                        {
                            indexResendedPost += offsetNextPost;
                            continue;
                        }

                        foreach (var tag in tagsNotRepost)
                        {
                            if (postText.Contains(tag))
                            {
                                indexResendedPost += offsetNextPost;
                                continue;
                            }
                        }

                        postText = string.Join("", tagsArr.Select(s => "#" + s + GROUP_SHORT_URL + "\n"));

                        postText = BaseTagsEditor.GetBaseTagsWithNextLine() + postText;

                        var mediaAttachmentList = new List<MediaAttachment>();

                        // Достать картинки из поста
                        foreach (var attachment in post.Attachments)
                        {
                            if (attachment.Type.Name == "Photo")
                            {
                                mediaAttachmentList.Add(new PhotoMy { OwnerId = attachment.Instance.OwnerId, Id = attachment.Instance.Id, AccessKey = attachment.Instance.AccessKey });
                            }
                        }

                        if (mediaAttachmentList.Count == 0)
                        {
                            indexResendedPost += offsetNextPost;
                            continue;
                        }

                        // Повторый пост
                        api.Wall.Post(new WallPostParams()
                        {
                            OwnerId = -1 * GROUP_ID,
                            FromGroup = true,
                            Message = '.' + postText,
                            Attachments = mediaAttachmentList,
                            PublishDate = firstPostData.Value.AddHours(1),

                        });

                        indexResendedPost += offsetNextPost;
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

        public static void SendVkPostToOther(object data)
        {
            var accessTokens = (Dictionary<string, string>)data;

            var vkApi = new VkApiCustom(accessTokens.GetValueOrDefault(GosUslugi.VK));

            long lastSendPostId = -1;

            var timeSleep = TimeSpan.FromMinutes(5);

            while (true)
            {
                try
                {
                    // Получение самого свежего поста
                    var wall = vkApi.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * GROUP_ID,
                        Count = 2,
                        Filter = WallFilter.All
                    });

                    Post post;
                    if (wall.WallPosts[0].IsPinned.Value)
                    {
                        post = wall.WallPosts[1];
                    }
                    else
                    {
                        post = wall.WallPosts[0];
                    }

                    // Проверка на новые посты
                    if (lastSendPostId == post.Id)
                    {
                        Thread.Sleep(timeSleep);
                        continue;
                    }
                    lastSendPostId = post.Id.Value;

                    // Проверка текста поста
                    var postText = post.Text;

                    if (postText.Contains('!'))
                    {
                        Thread.Sleep(timeSleep);
                        continue;
                    }

                    foreach (var tag in tagsNotRepost)
                    {
                        if (postText.Contains(tag))
                        {
                            Thread.Sleep(timeSleep);
                            continue;
                        }
                    }

                    var imagesUrl = new List<Uri>();

                    // Достать картинки из поста
                    foreach (var attachment in post.Attachments)
                    {
                        if (attachment.Type.Name == "Photo")
                        {
                            var photo = (Photo)attachment.Instance;
                            imagesUrl.Add(photo.Sizes.Last().Url);
                        }
                    }

                    if (imagesUrl.Count == 0)
                    {
                        Thread.Sleep(timeSleep);
                        continue;
                    }

                    using var wc = new WebClient();

                    // Истории
                    if (rand.Next(10) == 0)
                    {
                        wc.DownloadFile(imagesUrl.First(), "Story.jpg");

                        vkApi.Stories.Post(new VkNet.Model.GetPhotoUploadServerParams()
                        {
                            AddToNews = true,
                            GroupId = (ulong)GROUP_ID,
                            LinkText = StoryLinkText.Open,
                            LinkUrl = "https://vk.com/" + post.ToString().Replace("post", "wall")
                        }, accessTokens.GetValueOrDefault(GosUslugi.VK), "Story.jpg");
                    }

                    // Клипы

                    // Отправка в другие сети
                    var caption = TagsReplacer.ReplaceTagRemoveExcessFromVk(postText);
                    TelegramTask.PushPost(accessTokens.GetValueOrDefault(GosUslugi.TELEGRAM), caption, imagesUrl.ToArray());

                    //var discordCaption = BaseTagsEditor.RemoveBaseTags(caption);
                    //DiscordTask.PushPost(accessTokens.GetValueOrDefault(GosUslugi.DISCORD), discordCaption, imagesUrl.ToArray());
                }
                catch
                {
                    Thread.Sleep(timeSleep);
                    continue;
                }
            }
        }
    }
}
