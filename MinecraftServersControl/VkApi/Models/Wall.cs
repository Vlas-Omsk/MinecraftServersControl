using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Wall
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }
        public int ToId { get => OwnerId; set => OwnerId = value; }
        [JsonProperty("from_id")]
        public int FromId { get; private set; }
        [JsonProperty("created_by")]
        public int CreatedBy { get; private set; }
        [JsonProperty("date")]
        public int Date { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }
        [JsonProperty("reply_owner_id")]
        public int ReplyOwnerId { get; private set; }
        [JsonProperty("reply_post_id")]
        public int ReplyPostId { get; private set; }
        [JsonProperty("friends_only")]
        public bool FriendsOnly { get; private set; }
        [JsonProperty("comments")]
        public Comments Comments { get; private set; }
        [JsonProperty("copyright")]
        public Copyright Copyright { get; private set; }
        [JsonProperty("likes")]
        public WallLikes Likes { get; private set; }
        [JsonProperty("reposts")]
        public Reposts Reposts { get; private set; }
        [JsonProperty("views")]
        public Views Views { get; private set; }
        [JsonProperty("post_type")]
        public PostType PostType { get; private set; }
        [JsonProperty("post_source")]
        public PostSource PostSource { get; private set; }
        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; private set; }
        [JsonProperty("geo")]
        public WallGeo Geo { get; private set; }
        [JsonProperty("signer_id")]
        public int SignerId { get; private set; }
        [JsonProperty("copy_history")]
        public Wall CopyHistory { get; private set; }
        [JsonProperty("can_pin")]
        public bool CanPin { get; private set; }
        [JsonProperty("can_delete")]
        public bool CanDelete { get; private set; }
        [JsonProperty("can_edit")]
        public bool CanEdit { get; private set; }
        [JsonProperty("is_pinned")]
        public bool IsPinned { get; private set; }
        [JsonProperty("marked_as_ads")]
        public bool MarkedAsAds { get; private set; }
        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; private set; }
        [JsonProperty("donut")]
        public Donut Donut { get; private set; }
        [JsonProperty("postponed_id")]
        public int PostponedId { get; private set; }

        private Wall()
        {
        }
    }
}
