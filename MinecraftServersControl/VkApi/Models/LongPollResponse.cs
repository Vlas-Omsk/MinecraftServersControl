using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class LongPollResponse
    {
        [JsonProperty("failed")]
        public int Failed { get; private set; }
        [JsonProperty("ts")]
        public string Ts { get; private set; }
        [JsonProperty("updates")]
        public LongPollUpdate[] Updates { get; private set; }

        private LongPollResponse()
        {
        }
    }
}
