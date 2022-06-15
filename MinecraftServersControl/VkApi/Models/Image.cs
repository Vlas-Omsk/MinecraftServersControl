using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Image
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
    }
}
