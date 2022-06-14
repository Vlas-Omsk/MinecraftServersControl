using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class WallLikes
    {
        [JsonProperty("count")]
        public int Count { get; private set; }
        [JsonProperty("user_likes")]
        public bool UserLikes { get; private set; }
        [JsonProperty("can_like")]
        public bool CanLike { get; private set; }
        [JsonProperty("can_publish")]
        public bool CanPublish { get; private set; }

        private WallLikes()
        {
        }
    }
}
