using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class WallLikes
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("user_likes")]
        public bool UserLikes { get; set; }
        [JsonProperty("can_like")]
        public bool CanLike { get; set; }
        [JsonProperty("can_publish")]
        public bool CanPublish { get; set; }
    }
}
