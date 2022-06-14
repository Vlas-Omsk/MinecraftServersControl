using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Sticker
    {
        [JsonProperty("product_id")]
        public int ProductId { get; private set; }
        [JsonProperty("sticker_id")]
        public int StickerId { get; private set; }
        [JsonProperty("images")]
        public Image[] Images { get; private set; }
        [JsonProperty("images_with_background")]
        public Image[] ImagesWithBackground { get; private set; }
        [JsonProperty("animation_url")]
        public string AnimationUrl { get; private set; }
        [JsonProperty("is_allowed")]
        public bool IsAllowed { get; private set; }

        private Sticker()
        {
        }
    }
}
