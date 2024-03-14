using DataSet;
using DataSet.DataStruct;
using System.Net;
using VkNet.Model;

namespace DownloaderDataSetPhoto.Downloader
{
    internal sealed class DownloaderDataSetPhotoFromVK
    {
        private readonly VkApiCustom.VkApiCustom api;
        private readonly TagsLIst tagList;
        public DownloaderDataSetPhotoFromVK(VkApiCustom.VkApiCustom api, TagsLIst tagList)
        {
            this.api = api;
            this.tagList = tagList;
        }

        public void SavePhotosIdFromNewsfeed(string tag, int shiftPost, int countPhoto, Int64 ignorGroupId, float percentOriginalTag, string fileName, object lockNeuralNetworkResult)
        {
            NewsSearchResult newsfeedPosts;
            int indexPage = 0;
            newsfeedPosts = api.Newsfeed.Search(new NewsFeedSearchParams()
            {
                Query = tag,
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
                            SavePhotos(tag, post, ignorGroupId, percentOriginalTag, fileName, lockNeuralNetworkResult);
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
                        Query = tag,
                        Count = 200,
                        StartFrom = newsfeedPosts.NextFrom
                    });
                }

                indexPage++;
            } while (indexPage < 5);
        }

        private static readonly char[] separator = [' ', '@', ',', '\r', '\n'];
        private void SavePhotos(string currentTag, NewsSearchItem post, Int64 groupId, float percentOriginalTag, string fileName, object lockNeuralNetworkResult)
        {
            var stringList = new List<string>(10);

            if (post.OwnerId == (-1 * groupId))
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

            using var wc = new WebClient();
            foreach (var photo in photos)
            {
                wc.DownloadFile(photo.Sizes[2].Url, $"DATA_SET\\{fileName}.jpg");
                using var image = new Bitmap($"DATA_SET\\{fileName}.jpg");

                Directory.CreateDirectory("DATA_SET\\" + currentTag);

                lock (lockNeuralNetworkResult)
                {
                    if (NeuralNetwork.NeuralNetwork.NeuralNetworkResult(image, percentOriginalTag) == currentTag)
                    {
                        return;
                    }
                }

                var filesList = Directory.GetFiles("DATA_SET\\" + currentTag);
                bool similar = false;
                foreach (var file in filesList)
                {
                    var tmpImage = new Bitmap(file);
                    if (DataSetPhoto.IsSimilarPhoto(DataSetPhoto.ChangeResolution224224(image), DataSetPhoto.ChangeResolution224224(tmpImage)))
                    {
                        similar = true;
                    }
                }
                if (!similar)
                {
                    DataSetPhoto.Save(image, currentTag);
                }
            }
        }
    }
}
