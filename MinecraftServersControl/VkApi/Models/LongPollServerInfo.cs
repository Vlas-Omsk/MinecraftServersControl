using PinkJson2;
using System;

namespace VkApi.Models
{
    [Serializable]
    public sealed class LongPollServerInfo
    {
        [JsonProperty("key")]
        public string Key { get; private set; }
        [JsonProperty("server")]
        public string Server { get; private set; }
        [JsonProperty("ts")]
        public string Ts { get; private set; }

        private LongPollServerInfo()
        {
        }
    }
}
