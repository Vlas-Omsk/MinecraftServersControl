using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Doc
    {
        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }
        [JsonProperty("title")]
        public string Title { get; private set; }
        [JsonProperty("size")]
        public int Size { get; private set; }
        [JsonProperty("ext")]
        public string Extension { get; private set; }
        [JsonProperty("url")]
        public string Url { get; private set; }
        [JsonProperty("date")]
        public int Date { get; private set; }
        [JsonProperty("type")]
        public DocType Type { get; private set; }
        [JsonProperty("preview")]
        public Preview Preview { get; private set; }

        private Doc()
        {
        }
    }
}
