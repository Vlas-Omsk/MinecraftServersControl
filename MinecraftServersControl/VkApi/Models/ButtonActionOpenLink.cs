using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ButtonActionOpenLink : ButtonAction
    {
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }

        public override ButtonActionType Type => ButtonActionType.OpenLink;
    }
}
