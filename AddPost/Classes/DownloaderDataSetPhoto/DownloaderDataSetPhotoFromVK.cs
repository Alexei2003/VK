using AddPost.Classes.DataSet;
using MyCustomClasses;
using System.Net;
using VkNet.Model;

namespace AddPost.Classes.DownloaderDataSetPhoto
{
    internal class DownloaderDataSetPhotoFromVK
    {
        private readonly VkApiCustom api;
        public DownloaderDataSetPhotoFromVK(VkApiCustom api)
        {
            this.api = api;
        }

        public void SavePhotosIdFromNewsfeed(string tag, TagsLIst tagsList, int shiftPost, int countPhoto, Int64 ignorGroupId, float percentOriginalTag)
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

                newsfeedPosts = api.Newsfeed.Search(new NewsFeedSearchParams()
                {
                    Query = tag,
                    Count = 200,
                    StartFrom = newsfeedPosts.NextFrom
                });
                indexPage++;
            } while (indexPage < 5);
        }

        private void SavePhotos(string tag, NewsSearchItem post, Int64 groupId, float percentOriginalTag)
        {
            var stringList = new List<string>();

            if (post.OwnerId == (-1 * groupId))
            {
                return;
            }

            string str;
            foreach (var attachment in post.Attachments)
            {
                var mediaObject = attachment.Instance.ToString();
                if (mediaObject.Contains("photo"))
                {
                    stringList.Add(mediaObject.Replace("photo", ""));
                }
            }

            var photos = api.Photo.GetById(stringList, photoSizes: true);

            using WebClient wc = new WebClient();
            foreach (var photo in photos)
            {
                wc.DownloadFile(photo.Sizes[2].Url, "DATA_SET\\DataSet.jpg");
                using var image = new Bitmap("DATA_SET\\DataSet.jpg");

                //if(NeuralNetwork.NeuralNetworkResult(image, percentOriginalTag) != tag)
                {
                    DataSetPhoto.Add(image, tag);
                }
            }
        }
    }
}
