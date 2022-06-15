using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Reposts
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("user_reposted")]
        public bool UserReposted { get; set; }
    }
}
