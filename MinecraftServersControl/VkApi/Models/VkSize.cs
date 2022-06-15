using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VkSize
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("type")]
        public VkSizeType Type { get; set; }
    }
}
