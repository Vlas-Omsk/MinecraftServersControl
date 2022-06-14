using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class MessageAction
    {
        [JsonProperty("type")]
        public MessageActionType Type { get; private set; }
        [JsonProperty("member_id")]
        public int MemberId { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }
        [JsonProperty("email")]
        public string Email { get; private set; }
        [JsonProperty("photo")]
        public MessageActionPhoto Photo { get; private set; }

        private MessageAction()
        {
        }
    }
}
