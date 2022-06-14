using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum AttachmentType
    {
        Photo,
        Video,
        Audio,
        Doc,
        Link,
        Market,
        MarketAlbum,
        Wall,
        WallReply,
        Sticker,
        Gift
    }
}
