using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class WallGeo
    {
        [JsonProperty("type")]
        public string Type { get; private set; }
        [JsonProperty("coordinates")]
        public string Coordinates { get; private set; }
        [JsonProperty("place")]
        public Place Place { get; private set; }

        private WallGeo()
        {
        }
    }
}
