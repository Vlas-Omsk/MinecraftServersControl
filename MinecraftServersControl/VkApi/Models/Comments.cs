using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Comments
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("can_post")]
        public bool CanPost { get; set; }
        [JsonProperty("groups_can_post")]
        public bool GroupsCanPost { get; set; }
        [JsonProperty("can_close")]
        public bool CanClose { get; set; }
        [JsonProperty("can_open")]
        public bool CanOpen { get; set; }
    }
}
