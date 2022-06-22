using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Link
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("caption")]
        public string Caption { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("photo")]
        public Photo Photo { get; set; }
        [JsonProperty("product")]
        public Product Product { get; set; }
        [JsonProperty("button")]
        public LinkButton Button { get; set; }
        [JsonProperty("preview_page")]
        public string PreviewPage { get; set; }
        [JsonProperty("preview_url")]
        public string PreviewUrl { get; set; }
    }
}
