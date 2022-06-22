using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class TemplateElementAction
    {
        [JsonProperty("type")]
        public TemplateElementActionType Type { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
    }
}
