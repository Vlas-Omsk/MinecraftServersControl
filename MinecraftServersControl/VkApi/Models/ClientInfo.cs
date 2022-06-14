using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ClientInfo
    {
        [JsonProperty("button_actions")]
        public ButtonActionType[] ButtonActions { get; private set; }
        [JsonProperty("keyboard")]
        public bool Keyboard { get; private set; }
        [JsonProperty("inline_keyboard")]
        public bool InlineKeyboard { get; private set; }
        [JsonProperty("carousel")]
        public bool Carousel { get; private set; }
        [JsonProperty("lang_id")]
        public Lang Lang { get; private set; }

        private ClientInfo()
        {
        }
    }
}
