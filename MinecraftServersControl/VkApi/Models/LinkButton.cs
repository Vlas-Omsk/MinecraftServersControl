using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class LinkButton
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("action")]
        public LinkButtonAction Action { get; set; }
    }
}
