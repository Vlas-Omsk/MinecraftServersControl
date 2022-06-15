using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Attachment
    {
        [JsonProperty("type")]
        public AttachmentType Type { get; set; }
        [JsonProperty("photo")]
        public Photo Photo { get; set; }
        [JsonProperty("video")]
        public Video Video { get; set; }
        [JsonProperty("audio")]
        public Audio Audio { get; set; }
        [JsonProperty("doc")]
        public Doc Doc { get; set; }
        [JsonProperty("link")]
        public Link Link { get; set; }
        [JsonProperty("market")]
        public Market Market { get; set; }
        [JsonProperty("market_album")]
        public MarketAlbum MarketAlbum { get; set; }
        [JsonProperty("wall")]
        public Wall Wall { get; set; }
        [JsonProperty("wall_reply")]
        public WallReply WallReply { get; set; }
        [JsonProperty("sticker")]
        public Sticker Sticker { get; set; }
        [JsonProperty("gift")]
        public Gift Gift { get; set; }
    }
}
