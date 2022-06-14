using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class MarketAlbum
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("is_main")]
        public bool IsMain { get; private set; }
        [JsonProperty("is_hidden")]
        public bool IsHidden { get; private set; }
        [JsonProperty("photo")]
        public Photo Photo { get; private set; }
        [JsonProperty("count")]
        public int Count { get; private set; }

        private MarketAlbum()
        {
        }
    }
}
