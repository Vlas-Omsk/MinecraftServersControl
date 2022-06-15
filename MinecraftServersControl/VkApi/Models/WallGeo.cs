using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class WallGeo
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("coordinates")]
        public string Coordinates { get; set; }
        [JsonProperty("place")]
        public Place Place { get; set; }
    }
}
