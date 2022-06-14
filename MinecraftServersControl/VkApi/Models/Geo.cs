using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Geo
    {
        [JsonProperty("type")]
        public string Type { get; private set; }
        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; private set; }
        [JsonProperty("place")]
        public Place Place { get; private set; }
        [JsonProperty("showmap")]
        public bool ShowMap { get; private set; }

        private Geo()
        {
        }
    }
}
