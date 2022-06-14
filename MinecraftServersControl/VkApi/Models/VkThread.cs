using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VkThread
    {
        [JsonProperty("count")]
        public int Count { get; private set; }
        [JsonProperty("items")]
        public WallReply[] Items { get; private set; }
        [JsonProperty("can_post")]
        public bool CanPost { get; private set; }
        [JsonProperty("show_reply_button")]
        public bool ShowReplyButton { get; private set; }
        [JsonProperty("groups_can_post")]
        public bool GroupsCanPost { get; private set; }

        private VkThread()
        {
        }
    }
}
