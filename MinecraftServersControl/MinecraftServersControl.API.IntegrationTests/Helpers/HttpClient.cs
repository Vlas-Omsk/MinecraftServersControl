using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core.DTO;
using PinkJson2;
using System;
using System.Text;
using System.Threading.Tasks;
using WebLib;

namespace MinecraftServersControl.API.IntegrationTests.Helpers
{
    public sealed class HttpClient
    {
        private string _url;

        public HttpClient(string url)
        {
            _url = url;
        }

        public Task<HttpResponse<T>> GetResponse<T>(string path) where T : Result
        {
            return GetResponse<T>(path, null, null);
        }

        public Task<HttpResponse<T>> GetResponse<T>(string path, object data) where T : Result
        {
            return GetResponse<T>(path, data, null);
        }

        public async Task<HttpResponse<T>> GetResponse<T>(string path, object data, Guid? sessionId) where T : Result
        {
            var request = data != null ?
                HttpRequest.POST :
                HttpRequest.GET;
            request.Url = _url;
            request.SetPath(path);

            if (sessionId.HasValue)
                request.AddHeader("Authorization", sessionId.Value.ToString());

            if (data != null)
            {
                request.AddHeader("Content-Type", "application/json");
                request.SetData(data.Serialize().ToString(), Encoding.UTF8);
            }

            var response = await request.GetResponseAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK ||
                response.Headers[System.Net.HttpRequestHeader.ContentType].StartsWith("application/json"))
                return Json.Parse(response.GetStream(), Encoding.UTF8).Deserialize<HttpResponse<T>>();
            else
                return new HttpResponse<T>(response.GetText());
        }
    }
}
