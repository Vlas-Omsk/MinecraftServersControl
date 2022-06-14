using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Place
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("latitude")]
        public int Latitude { get; private set; }
        [JsonProperty("longitude")]
        public int Longitude { get; private set; }
        [JsonProperty("created")]
        public int Created { get; private set; }
        [JsonProperty("icon")]
        public string Icon { get; private set; }
        [JsonProperty("checkins")]
        public int Checkins { get; private set; }
        [JsonProperty("updated")]
        public int Updated { get; private set; }
        [JsonProperty("type")]
        public int Type { get; private set; }
        [JsonProperty("country")]
        public string Country { get; private set; }
        [JsonProperty("city")]
        public string City { get; private set; }
        [JsonProperty("address")]
        public string Address { get; private set; }

        private Place()
        {
        }
    }
}
