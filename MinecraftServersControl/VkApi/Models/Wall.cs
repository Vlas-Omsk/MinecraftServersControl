using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Wall
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        public int ToId { get => OwnerId; set => OwnerId = value; }
        [JsonProperty("from_id")]
        public int FromId { get; set; }
        [JsonProperty("created_by")]
        public int CreatedBy { get; set; }
        [JsonProperty("date")]
        public int Date { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("reply_owner_id")]
        public int ReplyOwnerId { get; set; }
        [JsonProperty("reply_post_id")]
        public int ReplyPostId { get; set; }
        [JsonProperty("friends_only")]
        public bool FriendsOnly { get; set; }
        [JsonProperty("comments")]
        public Comments Comments { get; set; }
        [JsonProperty("copyright")]
        public Copyright Copyright { get; set; }
        [JsonProperty("likes")]
        public WallLikes Likes { get; set; }
        [JsonProperty("reposts")]
        public Reposts Reposts { get; set; }
        [JsonProperty("views")]
        public Views Views { get; set; }
        [JsonProperty("post_type")]
        public PostType PostType { get; set; }
        [JsonProperty("post_source")]
        public PostSource PostSource { get; set; }
        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; set; }
        [JsonProperty("geo")]
        public WallGeo Geo { get; set; }
        [JsonProperty("signer_id")]
        public int SignerId { get; set; }
        [JsonProperty("copy_history")]
        public Wall CopyHistory { get; set; }
        [JsonProperty("can_pin")]
        public bool CanPin { get; set; }
        [JsonProperty("can_delete")]
        public bool CanDelete { get; set; }
        [JsonProperty("can_edit")]
        public bool CanEdit { get; set; }
        [JsonProperty("is_pinned")]
        public bool IsPinned { get; set; }
        [JsonProperty("marked_as_ads")]
        public bool MarkedAsAds { get; set; }
        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; set; }
        [JsonProperty("donut")]
        public Donut Donut { get; set; }
        [JsonProperty("postponed_id")]
        public int PostponedId { get; set; }
    }
}
