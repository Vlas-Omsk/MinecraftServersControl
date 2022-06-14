using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class MarketPrice
    {
        [JsonProperty("amount")]
        public string Amount { get; private set; }
        [JsonProperty("currency")]
        public Currency Currency { get; private set; }
        [JsonProperty("old_amount")]
        public string OldAmount { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }

        private MarketPrice()
        {
        }
    }
}
