using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Likes
    {
        [JsonProperty("count")]
        public int Count { get; private set; }
        [JsonProperty("user_likes")]
        public bool UserLikes { get; private set; }

        private Likes()
        {
        }
    }
}
