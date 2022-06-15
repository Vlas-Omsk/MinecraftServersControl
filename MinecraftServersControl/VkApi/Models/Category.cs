using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("section")]
        public Section Section { get; set; }
    }
}
