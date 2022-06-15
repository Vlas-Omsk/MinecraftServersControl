using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Geo
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }
        [JsonProperty("place")]
        public Place Place { get; set; }
        [JsonProperty("showmap")]
        public bool ShowMap { get; set; }
    }
}
