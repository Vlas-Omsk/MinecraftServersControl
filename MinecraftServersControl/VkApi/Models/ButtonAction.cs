using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ButtonAction
    {
        [JsonProperty("type")]
        public ButtonActionType Type { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("payload")]
        public string Payload { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("hash")]
        public string Hash { get; set; }
        [JsonProperty("app_id")]
        public int AppId { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
    }
}
