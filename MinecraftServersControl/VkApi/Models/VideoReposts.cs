using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VideoReposts
    {
        [JsonProperty("count")]
        public int Count { get; set; }
        [JsonProperty("wall_count")]
        public int WallCount { get; set; }
        [JsonProperty("mail_count")]
        public int MailCount { get; set; }
        [JsonProperty("user_reposted")]
        public bool UserReposted { get; set; }
    }
}
