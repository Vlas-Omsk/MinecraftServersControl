using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Keyboard
    {
        [JsonProperty("one_time")]
        public bool OneTime { get; private set; }
        [JsonProperty("buttons")]
        public Button[][] Buttons { get; private set; }
        [JsonProperty("inline")]
        public bool Inline { get; private set; }

        private Keyboard()
        {
        }
    }
}
