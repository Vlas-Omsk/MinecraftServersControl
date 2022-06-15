using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Section
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
