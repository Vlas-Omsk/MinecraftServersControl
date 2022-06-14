using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Price
    {
        [JsonProperty("amount")]
        public int Amount { get; private set; }
        [JsonProperty("currency")]
        public Currency Currency { get; private set; }
        [JsonProperty("text")]
        public string Text { get; private set; }

        private Price()
        {
        }
    }
}
