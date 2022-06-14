using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Graffiti
    {
        [JsonProperty("src")]
        public string Source { get; private set; }
        [JsonProperty("width")]
        public int Width { get; private set; }
        [JsonProperty("height")]
        public int Height { get; private set; }

        private Graffiti()
        {
        }
    }
}
