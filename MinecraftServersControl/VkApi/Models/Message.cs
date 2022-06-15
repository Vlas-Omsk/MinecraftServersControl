using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Message
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("date")]
        public int Date { get; set; }
        [JsonProperty("peer_id")]
        public int PeerId { get; set; }
        [JsonProperty("from_id")]
        public int FromId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("random_id")]
        public int RandomId { get; set; }
        [JsonProperty("ref")]
        public string Reference { get; set; }
        [JsonProperty("ref_source")]
        public string ReferenceSource { get; set; }
        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; set; }
        [JsonProperty("important")]
        public bool Important { get; set; }
        [JsonProperty("geo")]
        public Geo Geo { get; set; }
        [JsonProperty("payload")]
        public string Payload { get; set; }
        [JsonProperty("keyboard")]
        public Keyboard Keyboard { get; set; }
        [JsonProperty("fwd_messages")]
        public Message[] ForwardedMessages { get; set; }
        [JsonProperty("reply_message")]
        public Message ReplyMessage { get; set; }
        [JsonProperty("action")]
        public MessageAction Action { get; set; }
        [JsonProperty("admin_author_id")]
        public int AdminAuthorId { get; set; }
        [JsonProperty("conversation_message_id")]
        public int ConversationMessageId { get; set; }
        [JsonProperty("is_cropped")]
        public bool IsCropped { get; set; }
        [JsonProperty("members_count")]
        public int MembersCount { get; set; }
        [JsonProperty("update_time")]
        public int UpdateTime { get; set; }
        [JsonProperty("was_listened")]
        public bool WasListened { get; set; }
        [JsonProperty("pinned_at")]
        public int PinnedAt { get; set; }
        [JsonProperty("message_tag")]
        public string MessageTag { get; set; }
    }
}
