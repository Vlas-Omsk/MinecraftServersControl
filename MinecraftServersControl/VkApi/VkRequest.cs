using PinkJson2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkApi.Models;
using WebLib;

namespace VkApi
{
    public sealed class VkRequest
    {
        private readonly Dictionary<string, string> _collection = new Dictionary<string, string>();
        private string _url;

        internal VkRequest(string url)
        {
            _url = url;
        }

        public void SetParameter(string key, object value)
        {
            if (value == null)
                return;

            SetParameter(key, value.ToString());
        }

        public void SetParameter<T>(string key, IEnumerable<T> value)
        {
            if (value == null)
                return;

            SetParameter(key, string.Join(',', value));
        }

        public void SetParameter(string key, bool? value)
        {
            if (!value.HasValue)
                return;

            SetParameter(key, value.Value ? "1" : "0");
        }

        public void SetJsonParameter(string key, object value)
        {
            if (value == null)
                return;

            SetParameter(key, value.Serialize(VkClient.ObjectSerializerOptions).ToString());
        }

        public void SetParameter(string key, string value)
        {
            if (value == null)
                return;

            _collection[key] = value;
        }

        public async Task<T> GetResponse<T>()
        {
            return (await GetResponseInternal<VkResponse<T>>()).Response;
        }

        public Task GetResponse()
        {
            return GetResponseInternal<VkResponse>();
        }

        private async Task<T> GetResponseInternal<T>()
            where T : VkResponse
        {
            var httpRequest = HttpRequest.GET;
            httpRequest.Url = _url;
            httpRequest.Query = _collection;

            var httpResponse = await httpRequest.GetResponseAsync();

            using var stream = httpResponse.GetStream();
            var json = Json.Parse(stream, Encoding.UTF8);

            var response = json.Deserialize<T>(VkClient.ObjectSerializerOptions);

            if (response.Error != null)
                throw new VkResponseErrorException(response.Error);

            return response;
        }
    }
}
