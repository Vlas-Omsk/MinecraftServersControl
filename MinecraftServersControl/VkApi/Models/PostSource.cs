using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class PostSource
    {
        [JsonProperty("type")]
        public PostSourceType Type { get; private set; }
        [JsonProperty("platform")]
        public string Platform { get; private set; }
        [JsonProperty("data")]
        public PostSourceData Data { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }

        private PostSource()
        {
        }
    }
}
