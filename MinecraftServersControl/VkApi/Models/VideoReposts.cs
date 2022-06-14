using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class VideoReposts
    {
        [JsonProperty("count")]
        public int Count { get; private set; }
        [JsonProperty("wall_count")]
        public int WallCount { get; private set; }
        [JsonProperty("mail_count")]
        public int MailCount { get; private set; }
        [JsonProperty("user_reposted")]
        public bool UserReposted { get; private set; }

        private VideoReposts()
        {
        }
    }
}
