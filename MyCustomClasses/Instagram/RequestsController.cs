using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyCustomClasses.Instagram
{
    public class RequestsController
    {
        private readonly HttpClient httpClient = new HttpClient();

        public async Task<HttpResponseMessage> SendRequestAsync(string url, string accessToken)
        {
            var response = await httpClient.GetAsync($"{url}&access_token={accessToken}");
            return response;
        }

        public async Task<HttpResponseMessage> SendPostRequestAsync(string url, object data, string accessToken)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"{url}?access_token={accessToken}", content);
            return response;
        }

        public async Task<JObject> ParseResponseAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }
    }
}
