using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Copyright
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("link")]
        public string Link { get; private set; }
        [JsonProperty("name")]
        public string Name { get; private set; }
        [JsonProperty("type")]
        public string Type { get; private set; }

        private Copyright()
        {
        }
    }
}
