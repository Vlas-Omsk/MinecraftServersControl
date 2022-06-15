using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class WallReplyDonut
    {
        [JsonProperty("is_don")]
        public bool IsDonut { get; set; }
        [JsonProperty("placeholder")]
        public string Placeholder { get; set; }
    }
}
