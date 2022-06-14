using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Reposts
    {
        [JsonProperty("count")]
        public int Count { get; private set; }
        [JsonProperty("user_reposted")]
        public bool UserReposted { get; private set; }

        private Reposts()
        {
        }
    }
}
