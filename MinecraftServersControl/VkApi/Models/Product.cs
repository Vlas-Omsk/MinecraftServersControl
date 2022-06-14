using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Product
    {
        [JsonProperty("price")]
        public Price Price { get; private set; }

        private Product()
        {
        }
    }
}
