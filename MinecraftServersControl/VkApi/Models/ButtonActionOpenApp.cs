using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ButtonActionOpenApp : ButtonAction
    {
        [JsonProperty("app_id")]
        public int AppId { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("hash")]
        public string Hash { get; set; }

        public override ButtonActionType Type => ButtonActionType.OpenApp;
    }
}
