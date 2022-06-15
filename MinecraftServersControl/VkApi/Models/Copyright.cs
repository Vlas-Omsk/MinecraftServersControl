using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Copyright
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
