using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Message
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("date")]
        public int Date { get; private set; }
        [JsonProperty("peer_id")]
        public int PeerId { get; private set; }
        [JsonProperty("from_id")]
        public int FromId { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }
        [JsonProperty("random_id")]
        public int RandomId { get; private set; }
        [JsonProperty("ref")]
        public string Reference { get; private set; }
        [JsonProperty("ref_source")]
        public string ReferenceSource { get; private set; }
        [JsonProperty("attachments")]
        public Attachment[] Attachments { get; private set; }
        [JsonProperty("important")]
        public bool Important { get; private set; }
        [JsonProperty("geo")]
        public Geo Geo { get; private set; }
        [JsonProperty("payload")]
        public string Payload { get; private set; }
        [JsonProperty("keyboard")]
        public Keyboard Keyboard { get; private set; }
        [JsonProperty("fwd_messages")]
        public Message[] ForwardedMessages { get; private set; }
        [JsonProperty("reply_message")]
        public Message ReplyMessage { get; private set; }
        [JsonProperty("action")]
        public MessageAction Action { get; private set; }
        [JsonProperty("admin_author_id")]
        public int AdminAuthorId { get; private set; }
        [JsonProperty("conversation_message_id")]
        public int ConversationMessageId { get; private set; }
        [JsonProperty("is_cropped")]
        public bool IsCropped { get; private set; }
        [JsonProperty("members_count")]
        public int MembersCount { get; private set; }
        [JsonProperty("update_time")]
        public int UpdateTime { get; private set; }
        [JsonProperty("was_listened")]
        public bool WasListened { get; private set; }
        [JsonProperty("pinned_at")]
        public int PinnedAt { get; private set; }
        [JsonProperty("message_tag")]
        public string MessageTag { get; private set; }

        private Message()
        {
        }
    }
}
