using Newtonsoft.Json.Linq;

namespace MyCustomClasses.Instagram
{
    public class InstagramApi
    {
        private RequestsController requestsController = new RequestsController();
        private string accessToken;
        private string userId;

        private InstagramApi() { }

        public InstagramApi(string accessToken, string userId) : this()
        {
            Authorize(accessToken, userId);
        }

        public void Authorize(string accessToken, string userId)
        {
            this.accessToken = accessToken;
            this.userId = userId;
        }

        public async Task<JObject> CreateMediaObjectAsync(string imageUrl, string caption)
        {
            var createMediaData = new
            {
                image_url = imageUrl,
                caption = caption
            };

            var response = await requestsController.SendPostRequestAsync($"https://graph.instagram.com/v10.0/{userId}/media", createMediaData, accessToken);
            return await requestsController.ParseResponseAsync(response);
        }

        public async Task<JObject> PublishMediaAsync(string creationId)
        {
            var publishData = new
            {
                creation_id = creationId
            };

            var response = await requestsController.SendPostRequestAsync($"https://graph.instagram.com/v10.0/{userId}/media_publish", publishData, accessToken);
            return await requestsController.ParseResponseAsync(response);
        }
    }
}
