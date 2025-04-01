using System.Net;
using System.Text;

using DataSet;

using HtmlAgilityPack;

using MyCustomClasses;
using MyCustomClasses.Tags;
using MyCustomClasses.Tags.Editors;
using MyCustomClasses.VK;
using MyCustomClasses.VK.VKApiCustomClasses;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using VkNet.Enums.StringEnums;
using VkNet.Model;

namespace RepetitionOfPostsBot.BotTask
{
    internal class VKTask
    {
        private string _groupShortUrl;
        private long _groupId;
        private readonly string[] _tagsNotRepost;
        private readonly Dictionary<string, string> _accessTokens;
        private readonly VkApiCustom _vkApi;
        private readonly long _chatId;
        private readonly WebClient _wc = new WebClient();

        public VKTask(string groupShortUr, long groupId, long chatId, string[] tagsNotRepost, Dictionary<string, string> accessTokens)
        {
            _groupShortUrl = groupShortUr;
            _groupId = groupId;
            _tagsNotRepost = tagsNotRepost;
            _accessTokens = accessTokens;
            _chatId = chatId;

            _vkApi = new VkApiCustom(_accessTokens[GosUslugi.VK]);

            // Получение первого отложеного поста
            var wall = _vkApi.Wall.Get(new WallGetParams
            {
                OwnerId = -1 * _groupId,
                Count = 1,
                Filter = WallFilter.All
            });

            _offsetPost = GetRandomID(wall.TotalCount);
        }

        public void RunAll()
        {
#if !DEBUG
            Task.Run(() =>
            {
                RepeatVKPosts();
            });
            Task.Run(() =>
            {
                SendVkPostToOther();
            });
#endif
            Task.Run(() =>
            {
                CreateVkPostFromGelbooru();
            });
        }

        public static ulong GetRandomID(ulong max)
        {
            return Convert.ToUInt64(1 + RandomStatic.Rand.NextInt64(Convert.ToInt64(max)));
        }

        private readonly Queue<long> _sendPostIdQueue = [];
        private ulong _offsetPost;

        public void RepeatVKPosts()
        {
            const ulong COUNT_NOT_RESENDED_POST = 15;
            const ulong MAX_OFFSET_RESENDED_POST = 100;
            const ulong COUNT_GET_POSTS = 100;

            try
            {
                var wall = _vkApi.Wall.Get(new WallGetParams
                {
                    OwnerId = -1 * _groupId,
                    Count = 1,
                    Filter = WallFilter.Postponed
                });

                if (wall.WallPosts.Count < 1 || ((wall.WallPosts[0].Date.Value.Hour) > (DateTime.UtcNow.AddHours(1).Hour)))
                {
                    // Получение самого свежего поста
                    wall = _vkApi.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * _groupId,
                        Count = 2,
                        Filter = WallFilter.All
                    });

                    var lastPost = wall.WallPosts[0].IsPinned.Value ? wall.WallPosts[1] : wall.WallPosts[0];

                    var firstPostData = lastPost.Date;

                    var totalCountPosts = wall.TotalCount;
                    var notResendedPostCount = totalCountPosts / COUNT_NOT_RESENDED_POST;
                    var maxOffsetRessendedPost = totalCountPosts / MAX_OFFSET_RESENDED_POST;

                    List<MediaAttachment>? mediaAttachmentList = null;
                    string postText = "";
                    while (true)
                    {
                        var offsetNextPost = GetRandomID(maxOffsetRessendedPost) + COUNT_GET_POSTS;
                        _offsetPost += offsetNextPost;
                        _offsetPost %= totalCountPosts;

                        if (_offsetPost < notResendedPostCount)
                        {
                            _offsetPost += notResendedPostCount;
                        }

                        // Получение поста c offset 
                        wall = _vkApi.Wall.Get(new WallGetParams
                        {
                            OwnerId = -1 * _groupId,
                            Offset = _offsetPost,
                            Count = COUNT_GET_POSTS,
                            Filter = WallFilter.All
                        });

                        // Выход если поста несуществует
                        if (wall.WallPosts.Count == 0)
                        {
                            continue;
                        }

                        foreach (var post in wall.WallPosts)
                        {
                            mediaAttachmentList = [];

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

                            foreach (var tag in _tagsNotRepost)
                            {
                                if (postText.Contains(tag))
                                {
                                    continue;
                                }
                            }

                            postText = string.Join("", tagsArr.Select(s => "#" + s + _groupShortUrl + "\n"));

                            var bld = new StringBuilder();
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

                            if (_sendPostIdQueue.Contains(post.Id.Value))
                            {
                                continue;
                            }

                            if (_sendPostIdQueue.Count > 1000)
                            {
                                _sendPostIdQueue.Dequeue();
                            }
                            _sendPostIdQueue.Enqueue(post.Id.Value);



                            var publishDate = firstPostData.Value.AddHours(1);
                            while (publishDate < DateTime.UtcNow)
                            {
                                publishDate = publishDate.AddHours(1);
                            }

                            // Повторый пост
                            _vkApi.Wall.Post(new WallPostParams()
                            {
                                OwnerId = -1 * _groupId,
                                FromGroup = true,
                                Message = '.' + postText,
                                Attachments = mediaAttachmentList,
                                PublishDate = publishDate,

                            });

                            return;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
            }
        }

        private long _lastSendPostId = -1;
        public void SendVkPostToOther()
        {
            try
            {
                // Получение самого свежего поста
                var wall = _vkApi.Wall.Get(new WallGetParams
                {
                    OwnerId = -1 * _groupId,
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
                if (_lastSendPostId == post.Id)
                {
                    return;
                }
                _lastSendPostId = post.Id.Value;

                // Проверка текста поста
                var postText = post.Text;

                if (postText.Contains('!'))
                {
                    return;
                }

                foreach (var tag in _tagsNotRepost)
                {
                    if (postText.Contains(tag))
                    {
                        return;
                    }
                }

                var imagesUrl = new List<Uri>();

                // Достать картинки из поста
                foreach (var attachment in post.Attachments)
                {
                    if (attachment.Type.Name == "Photo")
                    {
                        var photo = (VkNet.Model.Photo)attachment.Instance;
                        imagesUrl.Add(photo.Sizes[^1].Url);
                    }
                }

                if (imagesUrl.Count == 0)
                {
                    return;
                }

                // Истории
                if (RandomStatic.Rand.Next(10) == 0)
                {
                    _wc.DownloadFile(imagesUrl[0], "Story.jpg");

                    Stories.Post(new GetPhotoUploadServerParams()
                    {
                        AddToNews = true,
                        GroupId = (ulong)_groupId,
                        LinkText = StoryLinkText.Open,
                        LinkUrl = "https://vk.com/" + post.ToString().Replace("post", "wall")
                    }, _accessTokens[GosUslugi.VK], "Story.jpg");
                }

                // Клипы

                // Отправка в другие сети
                var caption = TagsReplacer.ReplaceTagRemoveExcessFromVk(postText);
                TelegramTask.PushPost(_chatId, _accessTokens[GosUslugi.TELEGRAM], caption, imagesUrl.ToArray());
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
            }
        }

        private string _lastViewedUrl = "";
        private List<Task> _taskList =[];
        public void CreateVkPostFromGelbooru()
        {
            const string url = "https://gelbooru.com/index.php?page=post&s=list&tags=";

            var tmpLastViewedUrl = "";

            try
            {
                for (var i = 0; i < 10; i++)
                {
                    var htmlDocument = Gelbooru.GetPageHTML(_wc, url, i, useProxy: true);

                    var nodesArr = htmlDocument.DocumentNode
                        .SelectNodes("//a[@id and contains(@href, 'https') and contains(@href, 'gelbooru.com')]")
                        .ToArray();

                    if (i == 0)
                    {
                        tmpLastViewedUrl = nodesArr[0].GetAttributeValue("href", string.Empty);
                    }

                    if (!OpenArtsPage(nodesArr, _lastViewedUrl))
                    {
                        break;
                    }
                }

                Task.WaitAll(_taskList);
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
            }
            finally
            {
                _taskList.Clear();
            }
            CreatePost();
            _lastViewedUrl = tmpLastViewedUrl;

        }

        private bool OpenArtsPage(HtmlNode[] nodesArr, string lastViewedUrl)
        {
            foreach (var node in nodesArr)
            {
                var href = node.GetAttributeValue("href", string.Empty);

                if (href == lastViewedUrl)
                {
                    return false;
                }

                href = href.Replace("amp;", "");

                var htmlDocument = Gelbooru.GetPageHTML(_wc, href, useProxy: true);

                var nodesImageArr = htmlDocument.DocumentNode
                    .SelectNodes("//a[contains(text(), 'Original image')]")
                    .ToArray();

                // % знаки
                var nodeTagsArr = htmlDocument.DocumentNode
                    .SelectNodes("//li[contains(@class, 'tag-type-character')]/a[@href]")
                    ?.ToArray();

                if (nodeTagsArr == null)
                {
                    continue;
                }

                var task = Task.Run(() => 
                {
                    SaveImage(nodesImageArr[0], nodeTagsArr, _taskList.Count);
                });
                _taskList.Add(task);
            }
            return true;
        }

        private readonly Queue<Image<Rgb24>> _imageCheckedQueue = [];
        private readonly Queue<PhotoWithTag> _urlImageNotPostQueue = [];

        private sealed class PhotoWithTag
        {
            public string Tag { get; set; }

            public VkNet.Model.Photo Photo { get; set; }

            public PhotoWithTag(string tag, VkNet.Model.Photo photo)
            {
                Tag = tag;
                Photo = photo;
            }
        }

        private void SaveImage(HtmlNode nodeImage, HtmlNode[] nodeTags, int taskIndex)
        {
            const int COUNT_CHECKED_IMAGES = 10;
            const int COUNT_NOT_POST_IMAGES = 30;

            if (!Directory.Exists("Download"))
            {
                Directory.CreateDirectory("Download");
            }
            string path_image = Path.Combine("Download",$"Gelbooru-{taskIndex}.jpg");

            var href = nodeImage.GetAttributeValue("href", string.Empty);
            href = href.Replace("amp;", "");
            href = Gelbooru.GetUrlAddMirrorServer(href);

            var wc = new WebClient();
            wc.DownloadFile(href, path_image);
            var image = SixLabors.ImageSharp.Image.Load<Rgb24>(path_image);

            var resultTags = NeuralNetwork.NeuralNetworkWorker.NeuralNetworkResultKTopPercent(image);

            foreach (var nodeTag in nodeTags)
            {
                var tag = nodeTag.InnerText.Trim();
                var indexChar = tag.IndexOf('(');
                if (indexChar != -1)
                {
                    tag = tag.Remove(indexChar);
                }
                var tmpTag = new string([.. tag.Where(c => char.IsLetterOrDigit(c))]).ToLower();

                foreach (var resultTag in resultTags)
                {
                    var tmpResultTag = resultTag.Split('#', StringSplitOptions.RemoveEmptyEntries)[^1];
                    tmpResultTag = new string([.. tmpResultTag.Where(c => char.IsLetterOrDigit(c))]).ToLower();

                    if (tmpTag == tmpResultTag)
                    {
                        foreach (var checkedImage in _imageCheckedQueue)
                        {
                            if (DataSetImage.IsSimilarImage(checkedImage, image))
                            {
                                image.Dispose();
                                return;
                            }
                        }

                        if (_imageCheckedQueue.Count > COUNT_CHECKED_IMAGES)
                        {
                            using var imageDel = _imageCheckedQueue.Dequeue();
                        }
                        _imageCheckedQueue.Enqueue(image);

                        if (_urlImageNotPostQueue.Count > COUNT_NOT_POST_IMAGES)
                        {
                            _urlImageNotPostQueue.Dequeue();
                        }
                        _urlImageNotPostQueue.Enqueue(new PhotoWithTag(resultTag, _vkApi.Photo.AddOnVKServer(wc, image)[0]));

                        return;
                    }
                }
            }
        }

        private void CreatePost()
        {
            var countImagesPerPostLimit = int.Min((int)double.Ceiling(_urlImageNotPostQueue.Count / 2.0), 10);
            if (countImagesPerPostLimit > 0)
            {
                // Получение первого отложеного поста
                var wall = _vkApi.Wall.Get(new WallGetParams
                {
                    OwnerId = -1 * _groupId,
                    Count = 100,
                    Filter = WallFilter.Postponed
                });

                Post lastPost;
                if (wall.WallPosts.Count == 0)
                {
                    // Получение самого свежего поста
                    wall = _vkApi.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * _groupId,
                        Count = 2,
                        Filter = WallFilter.All
                    });

                    if (wall.WallPosts[0].IsPinned.Value)
                    {
                        lastPost = wall.WallPosts[1];
                    }
                    else
                    {
                        lastPost = wall.WallPosts[0];
                    }
                }
                else
                {
                    lastPost = wall.WallPosts[^1];
                }

                var publishDate = lastPost.Date.Value.AddHours(1);

                while (publishDate < DateTime.UtcNow)
                {
                    publishDate = publishDate.AddHours(1);
                }

                var imagesForSend = new List<PhotoWithTag>();

                for (var i = 0; i < countImagesPerPostLimit ; i++)
                {
                    imagesForSend.Add(_urlImageNotPostQueue.Dequeue());
                }

                var groups = imagesForSend
                    .Select(s => s.Tag.Split('#', StringSplitOptions.RemoveEmptyEntries)) // Разбиваем по #
                    .Where(parts => parts.Length == 2) // Убираем некорректные строки
                    .GroupBy(parts => parts[0], parts => parts[1]); // Группируем по Тайтлу

                var tags = groups.Select(g =>
                    $"#{g.Key}{_groupShortUrl}\n" + string.Join("\n", g.Distinct().Select(p => $"#{p}{_groupShortUrl}")) + "\n"
                );

                var resultTag = BaseTagsEditor.GetBaseTagsWithNextLine() + "\n" + string.Join("\n", tags);

                var imageList = imagesForSend.Select(s => s.Photo);
                // Новвый пост
                _vkApi.Wall.Post(new WallPostParams()
                {
                    OwnerId = -1 * _groupId,
                    FromGroup = true,
                    Message = resultTag,
                    Attachments = imageList,
                    PublishDate = publishDate,
                });
            }
        }
    }
}
