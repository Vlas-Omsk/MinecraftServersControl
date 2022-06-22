using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ButtonActionVkpay : ButtonAction
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        public override ButtonActionType Type => ButtonActionType.Vkpay;
    }
}
