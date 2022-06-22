using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class MessagesSendResult
    {
        [JsonProperty("peer_id")]
        public int PeerId { get; set; }
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        [JsonProperty("conversation_message_id")]
        public int ConversationMessageId { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
