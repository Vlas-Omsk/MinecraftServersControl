using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Currency
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
