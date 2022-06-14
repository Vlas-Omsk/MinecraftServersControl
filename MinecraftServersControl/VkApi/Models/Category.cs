using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Category
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("name")]
        public string Name { get; private set; }
        [JsonProperty("section")]
        public Section Section { get; private set; }

        private Category()
        {
        }
    }
}
