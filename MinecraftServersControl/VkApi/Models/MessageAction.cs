using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class MessageAction
    {
        [JsonProperty("type")]
        public MessageActionType Type { get; set; }
        [JsonProperty("member_id")]
        public int MemberId { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("photo")]
        public MessageActionPhoto Photo { get; set; }
    }
}
