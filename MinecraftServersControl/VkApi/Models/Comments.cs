using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Comments
    {
        [JsonProperty("count")]
        public int Count { get; private set; }
        [JsonProperty("can_post")]
        public bool CanPost { get; private set; }
        [JsonProperty("groups_can_post")]
        public bool GroupsCanPost { get; private set; }
        [JsonProperty("can_close")]
        public bool CanClose { get; private set; }
        [JsonProperty("can_open")]
        public bool CanOpen { get; private set; }

        private Comments()
        {
        }
    }
}
