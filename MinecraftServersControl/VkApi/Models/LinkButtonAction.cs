using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class LinkButtonAction
    {
        [JsonProperty("type")]
        public LinkButtonActionType Type { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
