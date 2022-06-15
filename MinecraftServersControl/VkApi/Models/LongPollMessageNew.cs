using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class LongPollMessageNew
    {
        [JsonProperty("message")]
        public Message Message { get; set; }
        [JsonProperty("client_info")]
        public ClientInfo Client { get; set; }
    }
}
