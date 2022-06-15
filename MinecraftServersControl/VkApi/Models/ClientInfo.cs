using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ClientInfo
    {
        [JsonProperty("button_actions")]
        public ButtonActionType[] ButtonActions { get; set; }
        [JsonProperty("keyboard")]
        public bool Keyboard { get; set; }
        [JsonProperty("inline_keyboard")]
        public bool InlineKeyboard { get; set; }
        [JsonProperty("carousel")]
        public bool Carousel { get; set; }
        [JsonProperty("lang_id")]
        public Language Language { get; set; }
    }
}
