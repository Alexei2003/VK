using DataSet.DataStruct;
using System.Net;
using VkNet.Model;

namespace DownloaderDataSetPhoto.Downloaders
{
    internal sealed class DownloaderDataSetPhotoFromVK
    {
        private readonly MyCustomClasses.VkApiCustom api;
        private readonly TagsLIst tagList;
        public DownloaderDataSetPhotoFromVK(MyCustomClasses.VkApiCustom api, TagsLIst tagList)
        {
            this.api = api;
            this.tagList = tagList;
        }

        public void SavePhotosFromNewsfeed(string currentTag, int shiftPost, int countPhoto, long ignorGroupId, float percentOriginalTag, string fileName, object lockNeuralNetworkResult)
        {
            using var wc = new WebClient();
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
                            SavePhotos(currentTag, post, ignorGroupId, percentOriginalTag, fileName, lockNeuralNetworkResult, wc);
                        }
                        catch (Exception ex)
                        {
                            continue;
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
        private void SavePhotos(string currentTag, NewsSearchItem post, long groupId, float percentOriginalTag, string fileName, object lockNeuralNetworkResult, WebClient wc)
        {
            var stringList = new List<string>(10);

            if (post.OwnerId == -1 * groupId)
            {
                return;
            }

            if (post.Attachments.Count != 1)
            {
                return;
            }

            var mediaObject = post.Attachments[0].Instance.ToString();
            if (mediaObject.Contains("photo"))
            {
                stringList.Add(mediaObject.Replace("photo", ""));
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
                tmpTag = tag.Split(separator, StringSplitOptions.RemoveEmptyEntries).First();
                if (!tagList.Find(tmpTag).IsEmpty)
                {
                    countFindTag++;
                }
            }

            if (countFindTag > 2)
            {
                return;
            }

            var photos = api.Photo.GetById(stringList, photoSizes: true);

            Downloader.DownloadPhoto(wc, photos[0].Sizes[2].Url.ToString(), currentTag, percentOriginalTag, fileName, lockNeuralNetworkResult);
        }
    }
}
