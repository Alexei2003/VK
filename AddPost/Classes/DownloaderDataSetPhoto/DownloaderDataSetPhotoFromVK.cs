using AddPost.Classes.DataSet;
using MyCustomClasses;
using System.Net;
using VkNet.Model;

namespace AddPost.Classes.DownloaderDataSetPhoto
{
    internal sealed class DownloaderDataSetPhotoFromVK
    {
        private readonly VkApiCustom api;
        public DownloaderDataSetPhotoFromVK(VkApiCustom api)
        {
            this.api = api;
        }

        public void SavePhotosIdFromNewsfeed(string tag, int shiftPost, int countPhoto, Int64 ignorGroupId, float percentOriginalTag)
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
                        SavePhotos(tag, post, ignorGroupId, percentOriginalTag);
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

        private void SavePhotos(string tag, NewsSearchItem post, Int64 groupId, float percentOriginalTag)
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

            var photos = api.Photo.GetById(stringList, photoSizes: true);

            using var wc = new WebClient();
            foreach (var photo in photos)
            {
                wc.DownloadFile(photo.Sizes[2].Url, "DATA_SET\\DataSet.jpg");
                using var image = new Bitmap("DATA_SET\\DataSet.jpg");

                Directory.CreateDirectory("DATA_SET\\" + tag);

                var filesList = Directory.GetFiles("DATA_SET\\" + tag);

                foreach (var file in filesList)
                {
                    using var tmpImage = new Bitmap(file);
                    if (DataSetPhoto.IsSimilarPhoto(DataSetPhoto.ChangeResolution(image), DataSetPhoto.ChangeResolution(tmpImage)))
                    {
                        return;
                    }
                }

                if (NeuralNetwork.NeuralNetworkResult(image, percentOriginalTag) != tag)
                {
                    DataSetPhoto.Save(image, tag);
                }
            }
        }
    }
}
