using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Video
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("duration")]
        public int Duration { get; set; }
        [JsonProperty("image")]
        public VideoImage[] Image { get; set; }
        [JsonProperty("first_frame")]
        public Image[] FirstFrame { get; set; }
        [JsonProperty("date")]
        public int Date { get; set; }
        [JsonProperty("adding_date")]
        public int AddingDate { get; set; }
        [JsonProperty("views")]
        public int Views { get; set; }
        [JsonProperty("local_views")]
        public int LocalViews { get; set; }
        [JsonProperty("comments")]
        public int Comments { get; set; }
        [JsonProperty("player")]
        public string Player { get; set; }
        [JsonProperty("platform")]
        public string Platform { get; set; }
        [JsonProperty("can_add")]
        public bool CanAdd { get; set; }
        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }
        [JsonProperty("access_key")]
        public string AccessKey { get; set; }
        [JsonProperty("processing")]
        public bool Processing { get; set; }
        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }
        [JsonProperty("can_comment")]
        public bool CanComment { get; set; }
        [JsonProperty("can_edit")]
        public bool CanEdit { get; set; }
        [JsonProperty("can_like")]
        public bool CanLike { get; set; }
        [JsonProperty("can_repost")]
        public bool CanRepost { get; set; }
        [JsonProperty("can_subscribe")]
        public bool CanSubscribe { get; set; }
        [JsonProperty("can_add_to_faves")]
        public bool CanAddToFaves { get; set; }
        [JsonProperty("can_attach_link")]
        public bool CanAttachLink { get; set; }
        [JsonProperty("width")]
        public int Width { get; set; }
        [JsonProperty("height")]
        public int Height { get; set; }
        [JsonProperty("user_id")]
        public int UserId { get; set; }
        [JsonProperty("converting")]
        public bool Converting { get; set; }
        [JsonProperty("added")]
        public bool Added { get; set; }
        [JsonProperty("is_subscribed")]
        public bool IsSubscribed { get; set; }
        [JsonProperty("repeat")]
        public bool Repeat { get; set; }
        [JsonProperty("type")]
        public VideoType Type { get; set; }
        [JsonProperty("balance")]
        public int Balance { get; set; }
        [JsonProperty("live_status")]
        public VideoLiveStatus LiveStatus { get; set; }
        [JsonProperty("live")]
        public bool Live { get; set; }
        [JsonProperty("upcoming")]
        public bool Upcoming { get; set; }
        [JsonProperty("spectators")]
        public int Spectators { get; set; }
        [JsonProperty("likes")]
        public Likes Likes { get; set; }
        [JsonProperty("reposts")]
        public VideoReposts Reposts { get; set; }
    }
}
