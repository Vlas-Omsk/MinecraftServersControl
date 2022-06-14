using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Market
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("description")]
        public string Description { get; private set; }
        [JsonProperty("price")]
        public MarketPrice Price { get; private set; }
        [JsonProperty("dimensions")]
        public Dimensions Dimensions { get; private set; }
        [JsonProperty("weight")]
        public int Weight { get; private set; }
        [JsonProperty("category")]
        public Category Category { get; private set; }
        [JsonProperty("thumb_photo")]
        public string ThumbPhoto { get; private set; }
        [JsonProperty("date")]
        public int Date { get; private set; }
        [JsonProperty("availability")]
        public MarketAvailability Availability { get; private set; }
        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; private set; }
        [JsonProperty("sku")]
        public string Sku { get; private set; }
        [JsonProperty("photos")]
        public Photo[] Photos { get; private set; }
        [JsonProperty("can_comment")]
        public bool CanComment { get; private set; }
        [JsonProperty("can_repost")]
        public bool CanRepost { get; private set; }
        [JsonProperty("likes")]
        public Likes Likes { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("button_title")]
        public string ButtonTitle { get; private set; }

        private Market()
        {
        }
    }
}
