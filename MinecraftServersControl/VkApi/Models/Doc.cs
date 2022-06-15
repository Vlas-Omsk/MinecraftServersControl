﻿using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Doc
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("owner_id")]
        public int OwnerId { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("size")]
        public int Size { get; set; }
        [JsonProperty("ext")]
        public string Extension { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("date")]
        public int Date { get; set; }
        [JsonProperty("type")]
        public DocType Type { get; set; }
        [JsonProperty("preview")]
        public Preview Preview { get; set; }
    }
}
