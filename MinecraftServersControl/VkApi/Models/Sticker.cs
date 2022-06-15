using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Sticker
    {
        [JsonProperty("product_id")]
        public int ProductId { get; set; }
        [JsonProperty("sticker_id")]
        public int StickerId { get; set; }
        [JsonProperty("images")]
        public Image[] Images { get; set; }
        [JsonProperty("images_with_background")]
        public Image[] ImagesWithBackground { get; set; }
        [JsonProperty("animation_url")]
        public string AnimationUrl { get; set; }
        [JsonProperty("is_allowed")]
        public bool IsAllowed { get; set; }
    }
}
