using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Likes
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("user_likes")]
        public bool UserLikes { get; set; }
    }
}
