using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Place
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("latitude")]
        public int Latitude { get; set; }
        [JsonProperty("longitude")]
        public int Longitude { get; set; }
        [JsonProperty("created")]
        public int Created { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("checkins")]
        public int Checkins { get; set; }
        [JsonProperty("updated")]
        public int Updated { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
    }
}
