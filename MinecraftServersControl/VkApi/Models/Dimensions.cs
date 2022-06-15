using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Dimensions
    {
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("length")]
        public int Length { get; set; }
    }
}
