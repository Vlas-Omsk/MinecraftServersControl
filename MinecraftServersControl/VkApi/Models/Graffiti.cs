using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Graffiti
    {
        [JsonProperty("src")]
        public string Source { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
    }
}
