using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Price
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
        [JsonProperty("currency")]
        public Currency Currency { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
