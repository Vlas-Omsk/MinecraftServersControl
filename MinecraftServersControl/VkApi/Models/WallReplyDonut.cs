using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class WallReplyDonut
    {
        [JsonProperty("is_don")]
        public bool IsDonut { get; private set; }
        [JsonProperty("placeholder")]
        public string Placeholder { get; private set; }

        private WallReplyDonut()
        {
        }
    }
}
