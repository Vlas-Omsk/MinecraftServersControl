using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class ButtonAction
    {
        [JsonProperty("type")]
        public ButtonActionType Type { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("label")]
        public string Label { get; private set; }
        [JsonProperty("payload")]
        public string Payload { get; private set; }
        [JsonProperty("link")]
        public string Link { get; private set; }
        [JsonProperty("hash")]
        public string Hash { get; private set; }
        [JsonProperty("app_id")]
        public int AppId { get; private set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }

        public ButtonAction()
        {
        }

        public ButtonAction(ButtonActionType type, string url, string label, string payload, string link, string hash, int appId, int ownerId)
        {
            Type = type;
            Url = url;
            Label = label;
            Payload = payload;
            Link = link;
            Hash = hash;
            AppId = appId;
            OwnerId = ownerId;
        }
    }
}
