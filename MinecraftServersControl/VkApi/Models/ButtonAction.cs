using PinkJson2;
using System;

namespace VkApi.Models
{
    public abstract class ButtonAction
    {
        [JsonProperty("type")]
        public abstract ButtonActionType Type { get; }
        [JsonProperty("payload")]
        public string Payload { get; set; }

        internal ButtonAction()
        {
        }
    }
}
