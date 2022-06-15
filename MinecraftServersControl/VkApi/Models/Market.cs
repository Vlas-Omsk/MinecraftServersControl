using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Market
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("price")]
        public MarketPrice Price { get; set; }
        [JsonProperty("dimensions")]
        public Dimensions Dimensions { get; set; }
        [JsonProperty("weight")]
        public int Weight { get; set; }
        [JsonProperty("category")]
        public Category Category { get; set; }
        [JsonProperty("thumb_photo")]
        public string ThumbPhoto { get; set; }
        [JsonProperty("date")]
        public int Date { get; set; }
        [JsonProperty("availability")]
        public MarketAvailability Availability { get; set; }
        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }
        [JsonProperty("sku")]
        public string Sku { get; set; }
        [JsonProperty("photos")]
        public Photo[] Photos { get; set; }
        [JsonProperty("can_comment")]
        public bool CanComment { get; set; }
        [JsonProperty("can_repost")]
        public bool CanRepost { get; set; }
        [JsonProperty("likes")]
        public Likes Likes { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("button_title")]
        public string ButtonTitle { get; set; }
    }
}
