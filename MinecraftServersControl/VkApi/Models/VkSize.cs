using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VkSize
    {
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("width")]
        public int Width { get; private set; }
        [JsonProperty("height")]
        public int Height { get; private set; }
        [JsonProperty("type")]
        public VkSizeType Type { get; private set; }

        private VkSize()
        {
        }
    }
}
