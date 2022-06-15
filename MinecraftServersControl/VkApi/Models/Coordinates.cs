using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Coordinates
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }
        [JsonProperty("longitude")]
        public double Longitude { get; set; }
    }
}
