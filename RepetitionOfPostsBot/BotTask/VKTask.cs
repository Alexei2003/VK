using System.Text;

using DataSet;

using HtmlAgilityPack;

using Other;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using VKClasses;
using VKClasses.Tags;
using VKClasses.Tags.Editors;
using VKClasses.VK;
using VKClasses.VK.VKApiCustomClasses;

using VkNet.Enums.Filters;
using VkNet.Enums.StringEnums;
using VkNet.Model;
using VkNet.Utils;

using User = VkNet.Model.User;

namespace RepetitionOfPostsBot.BotTask
{
    internal class VKTask
    {
        private string _vkGroupShortUr;
        private long _vkGroupId;
        private readonly string[] _tagsNotRepost;
        private readonly Dictionary<string, string> _accessTokens;
        private readonly VkApiCustom _vkApi;
        private readonly long _tgChatId;
        private readonly HttpClient _httpClient = new HttpClient();

        public VKTask(string vkGroupShortUr, long vkGroupId, long tgChatId, string[] tagsNotRepost, Dictionary<string, string> accessTokens)
        {
            _vkGroupShortUr = vkGroupShortUr;
            _vkGroupId = vkGroupId;
            _tagsNotRepost = tagsNotRepost;
            _accessTokens = accessTokens;
            _tgChatId = tgChatId;

            _vkApi = new VkApiCustom(_accessTokens[GosUslugi.VK]);

            // Получение первого отложеного поста
            var wall = _vkApi.Wall.Get(new WallGetParams
            {
                OwnerId = -1 * _vkGroupId,
                Count = 1,
                Filter = WallFilter.All
            });

            _offsetPost = GetRandomID(wall.TotalCount);
        }

        private int _time = 0;
        public void RunAll()
        {
#if !DEBUG
            Task.Run(() =>
            {
                SendVkPostToOther();
            });
#endif

#if !DEBUG
            Task.Run(() =>
            {
                if (!CreateVkPostFromGelbooru())
                {
                    RepeatVKPosts();
                }
            });
#endif

#if !DEBUG
            if (_time > 24)
            {
                Task.Run(() =>
                {
                   ClearPeople();
                });
                _time = 0;
            }
            else
            {
                _time++;
            }
#endif
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
                    OwnerId = -1 * _vkGroupId,
                    Count = 1,
                    Filter = WallFilter.Postponed
                });

                if (wall.WallPosts.Count < 1 || ((wall.WallPosts[0].Date.Value.Hour) > (DateTime.Now.AddHours(1).Hour)))
                {
                    // Получение самого свежего поста
                    wall = _vkApi.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * _vkGroupId,
                        Count = 2,
                        Filter = WallFilter.All
                    });

                    var lastPost = wall.WallPosts[0].IsPinned != null ? wall.WallPosts[1] : wall.WallPosts[0];

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
                            OwnerId = -1 * _vkGroupId,
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

                            postText = string.Join("", tagsArr.Select(s => "#" + s + _vkGroupShortUr + "\n"));

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
                                OwnerId = -1 * _vkGroupId,
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
                    OwnerId = -1 * _vkGroupId,
                    Count = 2,
                    Filter = WallFilter.All
                });

                Post post;
                if (wall.WallPosts[0].IsPinned != null)
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
                    _ = ImageTransfer.DownloadImageAsync(new HttpClient(), imagesUrl[0], "Story.jpg").Result;

                    _vkApi.Stories.Post(new GetPhotoUploadServerParams()
                    {
                        AddToNews = true,
                        GroupId = (ulong)_vkGroupId,
                        LinkText = StoryLinkText.Open,
                        LinkUrl = "https://vk.com/" + post.ToString().Replace("post", "wall")
                    }, "Story.jpg");
                }

                // Клипы

                // Отправка в другие сети
                var caption = TagsReplacer.ReplaceTagRemoveExcessFromVk(postText);
                TelegramTask.PushPost(_tgChatId, _accessTokens[GosUslugi.TELEGRAM], caption, imagesUrl.ToArray());
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
            }
        }

        private string _lastViewedUrl = "";
        private List<Task> _taskList = [];
        public bool CreateVkPostFromGelbooru()
        {
            if (RandomStatic.Rand.Next(12) == 0)
            {
                const string url = "https://gelbooru.com/index.php?page=post&s=list&tags=";

                var tmpLastViewedUrl = "";

                try
                {
                    for (var i = 0; i < 10; i++)
                    {
                        var htmlDocument = Gelbooru.GetPageHTML(_httpClient, url, i, useProxy: true);

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

                    _lastViewedUrl = tmpLastViewedUrl;
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
            }

            try
            {
                return CreatePost();
            }
            catch (Exception e)
            {
                Logs.WriteException(e);
                return false;
            }
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

                var htmlDocument = Gelbooru.GetPageHTML(_httpClient, href, useProxy: true);

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

                var taskIndex = _taskList.Count;
                var task = Task.Run(() =>
                {
                    try
                    {
                        SaveImage(nodesImageArr[0], nodeTagsArr, taskIndex);
                    }
                    catch (Exception ex)
                    {
                        Logs.WriteException(ex, "ERROR IN TASK");
                    }
                });
                _taskList.Add(task);
            }
            return true;
        }

        private readonly Queue<Image<Rgb24>> _checkedImageQueue = [];
        private readonly Queue<PhotoWithTag> _imageToPostQueue = [];

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

        private const int CountCheckedImage = 20;
        private const int CountImageToPost = 120;
        private void SaveImage(HtmlNode nodeImage, HtmlNode[] nodeTags, int taskIndex)
        {

            if (!Directory.Exists("Download"))
            {
                Directory.CreateDirectory("Download");
            }
            string path_image = Path.Combine("Download", $"Gelbooru-{taskIndex}.jpg");

            var href = nodeImage.GetAttributeValue("href", string.Empty);
            href = href.Replace("amp;", newValue: "");
            href = Gelbooru.GetUrlAddMirrorServer(href);

            var httpClient = new HttpClient();
            if (!ImageTransfer.DownloadImageAsync(httpClient, new Uri(href), path_image).Result)
            {
                return;
            }
            var image = SixLabors.ImageSharp.Image.Load<Rgb24>(path_image);

            if(image == null)
            {
                return;
            }

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

                    if (tmpTag.Contains(tmpResultTag) || tmpResultTag.Contains(tmpTag))
                    {
                        lock (_checkedImageQueue)
                        {
                            foreach (var checkedImage in _checkedImageQueue)
                            {
                                if (DataSetImage.IsSimilarImage(checkedImage, image))
                                {
                                    image.Dispose();
                                    return;
                                }
                            }

                            while (_checkedImageQueue.Count > CountCheckedImage)
                            {
                                using var imageDel = _checkedImageQueue.Dequeue();
                            }
                            _checkedImageQueue.Enqueue(image);
                        }

                        while (_imageToPostQueue.Count > CountImageToPost)
                        {
                            _imageToPostQueue.Dequeue();
                        }
                        var photo = _vkApi.Photo.AddOnVKServer(httpClient, path_image)?[0];

                        if(photo != null)
                        {
                            _imageToPostQueue.Enqueue(new PhotoWithTag(resultTag, photo));
                        }

                        return;
                    }
                }
            }
        }

        private bool CreatePost()
        {
            var countImagesPerPostLimit = _imageToPostQueue.Count / 2;

            if (countImagesPerPostLimit > 0)
            {
                var correctCount = new int[] { 9, 6, 4, 2, 1 };
                countImagesPerPostLimit = correctCount.First(i => i < countImagesPerPostLimit);

                // Получение первого отложеного поста
                var wall = _vkApi.Wall.Get(new WallGetParams
                {
                    OwnerId = -1 * _vkGroupId,
                    Count = 100,
                    Filter = WallFilter.Postponed
                });

                Post lastPost;
                if (wall.WallPosts.Count == 0)
                {
                    // Получение самого свежего поста
                    wall = _vkApi.Wall.Get(new WallGetParams
                    {
                        OwnerId = -1 * _vkGroupId,
                        Count = 2,
                        Filter = WallFilter.All
                    });

                    if (wall.WallPosts[0].IsPinned != null)
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

                for (var i = 0; i < countImagesPerPostLimit; i++)
                {
                    imagesForSend.Add(_imageToPostQueue.Dequeue());
                }

                var groups = imagesForSend
                    .Select(s => s.Tag.Split('#', StringSplitOptions.RemoveEmptyEntries)) // Разбиваем по #
                    .Where(parts => parts.Length == 2) // Убираем некорректные строки
                    .GroupBy(parts => parts[0], parts => parts[1]); // Группируем по Тайтлу

                var tags = groups.Select(g =>
                    $"#{g.Key}{_vkGroupShortUr}\n" + string.Join("\n", g.Distinct().Select(p => $"#{p}{_vkGroupShortUr}")) + "\n"
                );

                var resultTag = BaseTagsEditor.GetBaseTagsWithNextLine() + "\n" + string.Join("\n", tags);

                var imageList = imagesForSend.Select(s => s.Photo);
                // Новвый пост
                _vkApi.Wall.Post(new WallPostParams()
                {
                    OwnerId = -1 * _vkGroupId,
                    FromGroup = true,
                    Message = resultTag,
                    Attachments = imageList,
                    PublishDate = publishDate,
                });
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClearPeople()
        {
            VkCollection<User> members;
            int offset = 0;
            const int COUNT_USER = 1000;
#if DEBUG
            var count = 0;
#endif
            do
            {
                members = _vkApi.Groups.GetMembers(new GroupsGetMembersParams
                {
                    GroupId = _vkGroupId.ToString(),
                    Offset = offset,
                    Count = COUNT_USER,
                    Fields = UsersFields.All
                });
                offset += members.Count;

                foreach (var member in members)
                {
                    if (member.Deactivated != Deactivated.Activated || (member.LastSeen != null && member.LastSeen.Time < DateTime.Now.AddMonths(-1)))
                    {
                        _vkApi.Groups.RemoveUser(_vkGroupId, member.Id);
#if DEBUG
                        count++;
#endif
                    }
                }
            }
            while (members.Count == COUNT_USER);
#if DEBUG
            Console.WriteLine(count.ToString());
#endif
        }
    }
}
