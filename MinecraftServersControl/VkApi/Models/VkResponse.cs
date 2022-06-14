using PinkJson2;
using System;

namespace VkApi.Models
{
    public class VkResponse
    {
        [JsonProperty("error")]
        public VkResponseError Error { get; protected set; }

        protected VkResponse()
        {
        }
    }

    public class VkResponse<T> : VkResponse
    {
        [JsonProperty("response")]
        public T Response { get; protected set; }

        protected VkResponse()
        {
        }
    }
}
