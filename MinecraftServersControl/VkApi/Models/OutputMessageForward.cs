using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class OutputMessageForward
    {
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        [JsonProperty("peer_id")]
        public int PeerId { get; set; }
        [JsonProperty("conversation_message_ids")]
        public int[] ConversationMessageIds { get; set; }
        [JsonProperty("message_ids")]
        public int[] MessageIds { get; set; }
        [JsonProperty("is_reply")]
        public bool IsReply { get; set; }

        public OutputMessageForward()
        {
        }

        public OutputMessageForward(int ownerId, int peerId, int[] conversationMessageIds, int[] messageIds, bool isReply)
        {
            OwnerId = ownerId;
            PeerId = peerId;
            ConversationMessageIds = conversationMessageIds;
            MessageIds = messageIds;
            IsReply = isReply;
        }
    }
}
