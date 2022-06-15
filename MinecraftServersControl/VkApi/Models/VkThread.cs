using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VkThread
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("items")]
        public WallReply[] Items { get; set; }
        [JsonProperty("can_post")]
        public bool CanPost { get; set; }
        [JsonProperty("show_reply_button")]
        public bool ShowReplyButton { get; set; }
        [JsonProperty("groups_can_post")]
        public bool GroupsCanPost { get; set; }
    }
}
