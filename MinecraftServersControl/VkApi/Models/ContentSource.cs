using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ContentSource
    {
        [JsonProperty("type")]
        public ContentSourceType Type { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        [JsonProperty("peer_id")]
        public int PeerId { get; set; }
        [JsonProperty("conversation_message_id")]
        public int ConversationMessageId { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }

        public ContentSource(ContentSourceType type, int ownerId, int peerId, int conversationMessageId)
        {
            Type = type;
            OwnerId = ownerId;
            PeerId = peerId;
            ConversationMessageId = conversationMessageId;
        }

        public ContentSource(ContentSourceType type, string url)
        {
            Type = type;
            Url = url;
        }
    }
}
