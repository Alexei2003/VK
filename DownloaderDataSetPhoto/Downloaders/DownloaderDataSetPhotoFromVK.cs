using Other;

using VKClasses.Tags;
using VKClasses.VK;

using VkNet.Model;

namespace DownloaderDataSetPhoto.Downloaders
{
    public sealed class DownloaderDataSetPhotoFromVK
    {
        private readonly VkApiCustom api;
        private readonly TagsList tagList;
        public DownloaderDataSetPhotoFromVK(VkApiCustom api, TagsList tagList)
        {
            this.api = api;
            this.tagList = tagList;
        }

        public void SavePhotosFromNewsfeed(string currentTag, int shiftPost, int countPhoto, long ignorGroupId, string fileName)
        {
            using var httpClient = new HttpClient();
            NewsSearchResult newsfeedPosts;
            int indexPage = 0;
            newsfeedPosts = api.Newsfeed.Search(new NewsFeedSearchParams()
            {
                Query = currentTag,
                Count = 200,
            });

            do
            {
                foreach (var post in newsfeedPosts.Items)
                {
                    if (shiftPost < 1)
                    {
                        if (countPhoto < 1)
                        {
                            return;
                        }
                        else
                        {
                            countPhoto--;
                        }
                        try
                        {
                            SavePhotos(currentTag, post, ignorGroupId, fileName, httpClient);
                        }
                        catch (Exception e)
                        {
                            Logs.WriteException(e);
                        }
                    }
                    else
                    {
                        shiftPost--;
                    }
                }

                if (countPhoto < 1)
                {
                    return;
                }
                else
                {
                    newsfeedPosts = api.Newsfeed.Search(new NewsFeedSearchParams()
                    {
                        Query = currentTag,
                        Count = 200,
                        StartFrom = newsfeedPosts.NextFrom
                    });
                }

                indexPage++;
            } while (indexPage < 5);
        }

        private static readonly char[] separator = [' ', '@', ',', '\r', '\n'];
        private void SavePhotos(string currentTag, NewsSearchItem post, long groupId, string fileName, HttpClient httpClient)
        {
            if (post.OwnerId == -1 * groupId)
            {
                return;
            }

            if (post.Attachments.Count != 1)
            {
                return;
            }

            Photo photo;

            if (post.Attachments[0].Type == typeof(Photo))
            {
                photo = (Photo)post.Attachments[0].Instance;
            }
            else
            {
                return;
            }

            var tags = post.Text.Split('#', StringSplitOptions.RemoveEmptyEntries);
            int countFindTag = 0;
            string tmpTag;
            foreach (var tag in tags)
            {
                tmpTag = tag.Split(separator, StringSplitOptions.RemoveEmptyEntries)[0];
                if (!tagList.Find(tmpTag).IsEmpty)
                {
                    countFindTag++;
                }
            }

            if (countFindTag > 2)
            {
                return;
            }

            Downloader.DownloadPhoto(httpClient, new Uri(photo.Sizes[2].Url.ToString()), currentTag, fileName);
        }
    }
}
