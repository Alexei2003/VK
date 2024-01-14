using System.Collections.ObjectModel;
using VkNet;
using VkNet.Model;

namespace MyCustomClasses.VkApiCustomClasses
{
    public class Photo
    {
        private readonly TimeSpan TIME_SLEEP;
        public VkApi ApiOriginal { get; }

        public Photo(VkApi ApiOriginal, TimeSpan TIME_SLEEP)
        {
            this.ApiOriginal = ApiOriginal;
            this.TIME_SLEEP = TIME_SLEEP;
        }

        public UploadServerInfo GetWallUploadServer(long? groupId = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Photo.GetWallUploadServer(groupId);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }

        public ReadOnlyCollection<VkNet.Model.Photo> SaveWallPhoto(string response, ulong? userId, ulong? groupId = null, string caption = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Photo.SaveWallPhoto(response, userId, groupId, caption);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }
        public ReadOnlyCollection<VkNet.Model.Photo> GetById(IEnumerable<string> photos, bool? extended = null, bool? photoSizes = null)
        {
            while (true)
            {
                try
                {
                    var res = ApiOriginal.Photo.GetById(photos, extended, photoSizes);
                    return res;
                }
                catch (VkNet.Exception.TooManyRequestsException)
                {
                    Thread.Sleep(TIME_SLEEP);
                }
            }
        }
    }
}
