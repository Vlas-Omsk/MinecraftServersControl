using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class LongPollResponse
    {
        [JsonProperty("failed")]
        public int Failed { get; set; }
        [JsonProperty("ts")]
        public string Ts { get; set; }
        [JsonProperty("updates")]
        public LongPollUpdate[] Updates { get; set; }
    }
}
