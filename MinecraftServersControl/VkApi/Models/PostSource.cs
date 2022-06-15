using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class PostSource
    {
        [JsonProperty("type")]
        public PostSourceType Type { get; set; }
        [JsonProperty("platform")]
        public string Platform { get; set; }
        [JsonProperty("data")]
        public PostSourceData Data { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
