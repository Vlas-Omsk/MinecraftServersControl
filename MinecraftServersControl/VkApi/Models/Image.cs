using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Image
    {
        [JsonProperty("height")]
        public int Height { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("width")]
        public int Width { get; private set; }

        private Image()
        {
        }
    }
}
