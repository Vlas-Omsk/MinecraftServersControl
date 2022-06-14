using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VideoImage
    {
        [JsonProperty("height")]
        public int Height { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("width")]
        public int Width { get; private set; }
        [JsonProperty("with_padding")]
        public bool WithPadding { get; private set; }

        private VideoImage()
        {
        }
    }
}
