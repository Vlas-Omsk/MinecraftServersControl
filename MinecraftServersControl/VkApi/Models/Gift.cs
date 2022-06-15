using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Gift
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("thumb_256")]
        public string Thumb256 { get; set; }
        [JsonProperty("thumb_96")]
        public string Thumb96 { get; set; }
        [JsonProperty("thumb_48")]
        public string Thumb48 { get; set; }
    }
}
