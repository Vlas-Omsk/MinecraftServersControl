using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Views
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
