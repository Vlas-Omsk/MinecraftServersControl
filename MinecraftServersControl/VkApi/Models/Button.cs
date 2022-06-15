using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Button
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("action")]
        public ButtonAction Action { get; set; }
        [JsonProperty("color")]
        public ButtonColor Color { get; set; }
    }
}
