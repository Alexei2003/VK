﻿using HtmlAgilityPack;
using MyCustomClasses;
using MyCustomClasses.Tags;
using MyCustomClasses.Tags.Editors;
using MyCustomClasses.VK;
using System.Drawing;
using System.Net;
using System.Text;
using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace RepetitionOfPostsBot.BotTask
{
    internal static class VKTask
    {
        private const string GROUP_SHORT_URL = "@anime_art_for_every_day";
        private const long GROUP_ID = 220199532;
        private static readonly TimeSpan TIME_SLEEP_ERROR = TimeSpan.FromMinutes(15);

        private static readonly string[] tagsNotRepost = ["Угадайка"];

        public static ulong GetRandomID(ulong max)
        {
            return Convert.ToUInt64(1 + RandomStatic.Rand.NextInt64(Convert.ToInt64(max)));
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

            ulong offsetPost = GetRandomID(wall.TotalCount);

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

                    if (wall.WallPosts.Count < 1 || ((wall.WallPosts[0].Date.Value.Hour) > (DateTime.UtcNow.AddHours(1).Hour)))
                    {
                        // Получение самого свежего поста
                        wall = api.Wall.Get(new WallGetParams
                        {
                            OwnerId = -1 * GROUP_ID,
                            Count = 2,
                            Filter = WallFilter.All
                        });

                        Post firstPost;
                        if (wall.WallPosts[0].IsPinned.Value)
                        {
                            firstPost = wall.WallPosts[1];
                        }
                        else
                        {
                            firstPost = wall.WallPosts[0];
                        }

                        var firstPostData = firstPost.Date;

                        var totalCountPosts = wall.TotalCount;
                        var notResendedCountPosts = totalCountPosts / 15;
                        var maxRandomOffsetRessendedPosts = totalCountPosts / 5;

                        List<MediaAttachment>? mediaAttachmentList = null;
                        string postText = "";
                        while (true)
                        {
                            var offsetNextPost = GetRandomID(maxRandomOffsetRessendedPosts);
                            offsetPost += offsetNextPost;
                            offsetPost %= totalCountPosts;

                            if (offsetPost < notResendedCountPosts)
                            {
                                offsetPost += notResendedCountPosts;
                            }

                            // Получение поста по id 
                            wall = api.Wall.Get(new WallGetParams
                            {
                                OwnerId = -1 * GROUP_ID,
                                Offset = offsetPost,
                                Count = 100,
                                Filter = WallFilter.All
                            });

                            bool postFind = false;
                            foreach (var post in wall.WallPosts)
                            {
                                mediaAttachmentList = [];

                                // Выход если поста несуществует
                                if (wall.WallPosts.Count == 0)
                                {
                                    continue;
                                }

                                postText = post.Text;

                                // Проверка тега
                                postText = BaseTagsEditor.RemoveBaseTags(postText);
                                postText = TagsReplacer.RemoveGroupLinkFromTag(postText);
                                postText = postText.Replace("\n", "");

                                var tagsArr = postText.Split('#', StringSplitOptions.RemoveEmptyEntries);

                                if (tagsArr.Length == 0 || postText.Contains('.') || postText.Contains(' ') || postText.Contains('!'))
                                {
                                    continue;
                                }

                                foreach (var tag in tagsNotRepost)
                                {
                                    if (postText.Contains(tag))
                                    {
                                        continue;
                                    }
                                }

                                postText = string.Join("", tagsArr.Select(s => "#" + s + GROUP_SHORT_URL + "\n"));

                                StringBuilder bld = new StringBuilder();
                                bld.Append(BaseTagsEditor.GetBaseTagsWithNextLine());
                                bld.Append(postText);
                                postText = bld.ToString();

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
                                    continue;
                                }

                                postFind = true;
                                break;
                            }
                            if (postFind)
                            {
                                break;
                            }
                        }

                        var publishDate = firstPostData.Value.AddHours(1);

                        // Повторый пост
                        api.Wall.Post(new WallPostParams()
                        {
                            OwnerId = -1 * GROUP_ID,
                            FromGroup = true,
                            Message = '.' + postText,
                            Attachments = mediaAttachmentList,
                            PublishDate = publishDate < DateTime.UtcNow ? publishDate.AddHours(DateTime.UtcNow.Hour - publishDate.Hour + 1) : publishDate,

                        });
                    }

                    Thread.Sleep(TimeSpan.FromMinutes(30));
                }
                catch (Exception e)
                {
                    Logs.WriteExcemption(e);
                    Thread.Sleep(TIME_SLEEP_ERROR);
                    continue;
                }
            }
        }

        public static void SendVkPostToOther(object data)
        {
            var accessTokens = (Dictionary<string, string>)data;

            var vkApi = new VkApiCustom(accessTokens.GetValueOrDefault(GosUslugi.VK));

            long lastSendPostId = -1;

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
                        Thread.Sleep(TIME_SLEEP_ERROR);
                        continue;
                    }
                    lastSendPostId = post.Id.Value;

                    // Проверка текста поста
                    var postText = post.Text;

                    if (postText.Contains('!'))
                    {
                        Thread.Sleep(TIME_SLEEP_ERROR);
                        continue;
                    }

                    foreach (var tag in tagsNotRepost)
                    {
                        if (postText.Contains(tag))
                        {
                            Thread.Sleep(TIME_SLEEP_ERROR);
                        }
                    }

                    var imagesUrl = new List<Uri>();

                    // Достать картинки из поста
                    foreach (var attachment in post.Attachments)
                    {
                        if (attachment.Type.Name == "Photo")
                        {
                            var photo = (Photo)attachment.Instance;
                            imagesUrl.Add(photo.Sizes[^1].Url);
                        }
                    }

                    if (imagesUrl.Count == 0)
                    {
                        Thread.Sleep(TIME_SLEEP_ERROR);
                        continue;
                    }

                    using var wc = new WebClient();

                    // Истории
                    if (RandomStatic.Rand.Next(10) == 0)
                    {
                        wc.DownloadFile(imagesUrl[0], "Story.jpg");

                        MyCustomClasses.VK.VKApiCustomClasses.Stories.Post(new VkNet.Model.GetPhotoUploadServerParams()
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
                }
                catch (Exception e)
                {
                    Logs.WriteExcemption(e);
                    Thread.Sleep(TIME_SLEEP_ERROR);
                }
            }
        }

        public static void CreateVkPostFromGelbooru(object data)
        {
            var api = new VkApiCustom((string)data);

            var url = "https://gelbooru.com/index.php?page=post&s=list&tags=";
            //var url = "https://gelbooru.com/index.php?page=post&s=list&tags=azur_lane";

            using var wc = new WebClient();
            var lastViewedUrl = "";
            var tmpLastViewedUrl = "";
            while (true)
            {
                try
                {
                    for (var i = 0; i < 10; i++)
                    {
                        var htmlDocument = Gelbooru.GetPageHTML(wc, url, i);

                        var nodesArr = Gelbooru.GetObjectsNodes(htmlDocument, "//a[@id and @href]", ["https", "img3.gelbooru.com"]);

                        if (i == 0)
                        {
                            tmpLastViewedUrl = nodesArr[0].GetAttributeValue("href", string.Empty);
                        }

                        if (!OpenArtsPage(wc, nodesArr, lastViewedUrl))
                        {
                            break;
                        }
                    }

                    lastViewedUrl = tmpLastViewedUrl;
                    Thread.Sleep(TimeSpan.FromMinutes(30));
                }
                catch (Exception e)
                {
                    lastViewedUrl = tmpLastViewedUrl;
                    Logs.WriteExcemption(e);
                    Thread.Sleep(TIME_SLEEP_ERROR);
                }
            }
        }

        private static bool OpenArtsPage(WebClient wc, HtmlNode[] nodesArr, string lastViewedUrl)
        {
            foreach (var node in nodesArr)
            {
                var href = node.GetAttributeValue("href", string.Empty);

                if (href == lastViewedUrl)
                {
                    return false;
                }

                href = href.Replace("amp;", "");

                var htmlDocument = Gelbooru.GetPageHTML(wc, href, addNoSearch: false);

                var nodesImageArr = Gelbooru.GetObjectsNodes(htmlDocument, "//a[@href]", ["https", "img3.gelbooru.com", "Original image"]);
                var nodeTagsArr = Gelbooru.GetObjectsNodes(htmlDocument, "//a[@href]", ["s=list", "tags="]);

                SaveImage(wc, nodesImageArr[0], nodeTagsArr);
            }
            return true;
        }

        private static void SaveImage(WebClient wc, HtmlNode nodeImage, HtmlNode[] nodeTags)
        {
            var href = nodeImage.GetAttributeValue("href", string.Empty);

            wc.DownloadFile(href, $"Gelbooru.jpg");
            using var image = new Bitmap($"Gelbooru.jpg");

            var resultTags = NeuralNetwork.NeuralNetwork.NeuralNetworkResult(image, 5);

            var charsToRemove = new HashSet<char> { '\'', '_' };

            foreach (var resultTag in resultTags)
            {
                var tmpResultTag = new string(resultTag.Where(c => !charsToRemove.Contains(c)).ToArray()).ToUpper();
                foreach (var nodeTag in nodeTags)
                {
                    var tag = nodeTag.InnerText.Trim();
                    var tmpTag = new string(tag.Where(c => !charsToRemove.Contains(c)).ToArray()).ToUpper();

                    if (tmpTag.Contains(tmpResultTag))
                    {
                        var a = 1;
                    }
                }
            }
        }

        private static void CreatePost()
        {

        }
    }
}
