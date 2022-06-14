using PinkJson2;
using System;

namespace VkApi.Models
{
    public sealed class Attachment
    {
        [JsonProperty("type")]
        public AttachmentType Type { get; private set; }
        [JsonProperty("photo")]
        public Photo Photo { get; private set; }
        [JsonProperty("video")]
        public Video Video { get; private set; }
        [JsonProperty("audio")]
        public Audio Audio { get; private set; }
        [JsonProperty("doc")]
        public Doc Doc { get; private set; }
        [JsonProperty("link")]
        public Link Link { get; private set; }
        [JsonProperty("market")]
        public Market Market { get; private set; }
        [JsonProperty("market_album")]
        public MarketAlbum MarketAlbum { get; private set; }
        [JsonProperty("wall")]
        public Wall Wall { get; private set; }
        [JsonProperty("wall_reply")]
        public WallReply WallReply { get; private set; }
        [JsonProperty("sticker")]
        public Sticker Sticker { get; private set; }
        [JsonProperty("gift")]
        public Gift Gift { get; private set; }

        private Attachment()
        {
        }
    }
}
