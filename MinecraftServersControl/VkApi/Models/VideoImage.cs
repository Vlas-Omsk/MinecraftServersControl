using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VideoImage
    {
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("with_padding")]
        public bool WithPadding { get; set; }
    }
}
