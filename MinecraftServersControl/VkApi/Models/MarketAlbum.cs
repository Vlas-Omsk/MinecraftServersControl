using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class MarketAlbum
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("is_main")]
        public bool IsMain { get; set; }
        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }
        [JsonProperty("photo")]
        public Photo Photo { get; set; }
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
