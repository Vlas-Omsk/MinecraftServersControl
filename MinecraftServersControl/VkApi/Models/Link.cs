using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Link
    {
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("caption")]
        public string Caption { get; private set; }
        [JsonProperty("description")]
        public string Description { get; private set; }
        [JsonProperty("photo")]
        public Photo Photo { get; private set; }
        [JsonProperty("product")]
        public Product Product { get; private set; }
        [JsonProperty("button")]
        public Button Button { get; private set; }
        [JsonProperty("preview_page")]
        public string PreviewPage { get; private set; }
        [JsonProperty("preview_url")]
        public string PreviewUrl { get; private set; }

        private Link()
        {
        }
    }
}
