using PinkJson2;
using System;

namespace VkApi.Models
{
    [Serializable]
    public sealed class LongPollServerInfo
    {
        [JsonProperty("key")]
        public string Key { get; set; }
        [JsonProperty("server")]
        public string Server { get; set; }
        [JsonProperty("ts")]
        public string Ts { get; set; }
    }
}
