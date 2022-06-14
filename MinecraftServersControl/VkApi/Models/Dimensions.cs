using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Dimensions
    {
        [JsonProperty("width")]
        public int Width { get; private set; }
        [JsonProperty("height")]
        public int Height { get; private set; }
        [JsonProperty("length")]
        public int Length { get; private set; }

        private Dimensions()
        {
        }
    }
}
