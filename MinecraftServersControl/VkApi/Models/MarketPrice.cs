using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class MarketPrice
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("currency")]
        public Currency Currency { get; set; }
        [JsonProperty("old_amount")]
        public string OldAmount { get; set; }
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
