using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Keyboard
    {
        [JsonProperty("one_time")]
        public bool OneTime { get; set; }
        [JsonProperty("buttons")]
        public Button[][] Buttons { get; set; }
        [JsonProperty("inline")]
        public bool Inline { get; set; }
    }
}
