using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ButtonActionCallback : ButtonAction
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        public override ButtonActionType Type => ButtonActionType.Callback;
    }
}
