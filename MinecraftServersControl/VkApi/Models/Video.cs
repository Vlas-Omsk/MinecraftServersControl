using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Video
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("description")]
        public string Description { get; private set; }
        [JsonProperty("duration")]
        public int Duration { get; private set; }
        [JsonProperty("image")]
        public VideoImage[] Image { get; private set; }
        [JsonProperty("first_frame")]
        public Image[] FirstFrame { get; private set; }
        [JsonProperty("date")]
        public int Date { get; private set; }
        [JsonProperty("adding_date")]
        public int AddingDate { get; private set; }
        [JsonProperty("views")]
        public int Views { get; private set; }
        [JsonProperty("local_views")]
        public int LocalViews { get; private set; }
        [JsonProperty("comments")]
        public int Comments { get; private set; }
        [JsonProperty("player")]
        public string Player { get; private set; }
        [JsonProperty("platform")]
        public string Platform { get; private set; }
        [JsonProperty("can_add")]
        public bool CanAdd { get; private set; }
        [JsonProperty("is_private")]
        public bool IsPrivate { get; private set; }
        [JsonProperty("access_key")]
        public string AccessKey { get; private set; }
        [JsonProperty("processing")]
        public bool Processing { get; private set; }
        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; private set; }
        [JsonProperty("can_comment")]
        public bool CanComment { get; private set; }
        [JsonProperty("can_edit")]
        public bool CanEdit { get; private set; }
        [JsonProperty("can_like")]
        public bool CanLike { get; private set; }
        [JsonProperty("can_repost")]
        public bool CanRepost { get; private set; }
        [JsonProperty("can_subscribe")]
        public bool CanSubscribe { get; private set; }
        [JsonProperty("can_add_to_faves")]
        public bool CanAddToFaves { get; private set; }
        [JsonProperty("can_attach_link")]
        public bool CanAttachLink { get; private set; }
        [JsonProperty("width")]
        public int Width { get; private set; }
        [JsonProperty("height")]
        public int Height { get; private set; }
        [JsonProperty("user_id")]
        public int UserId { get; private set; }
        [JsonProperty("converting")]
        public bool Converting { get; private set; }
        [JsonProperty("added")]
        public bool Added { get; private set; }
        [JsonProperty("is_subscribed")]
        public bool IsSubscribed { get; private set; }
        [JsonProperty("repeat")]
        public bool Repeat { get; private set; }
        [JsonProperty("type")]
        public VideoType Type { get; private set; }
        [JsonProperty("balance")]
        public int Balance { get; private set; }
        [JsonProperty("live_status")]
        public VideoLiveStatus LiveStatus { get; private set; }
        [JsonProperty("live")]
        public bool Live { get; private set; }
        [JsonProperty("upcoming")]
        public bool Upcoming { get; private set; }
        [JsonProperty("spectators")]
        public int Spectators { get; private set; }
        [JsonProperty("likes")]
        public Likes Likes { get; private set; }
        [JsonProperty("reposts")]
        public VideoReposts Reposts { get; private set; }

        private Video()
        {
        }
    }
}
