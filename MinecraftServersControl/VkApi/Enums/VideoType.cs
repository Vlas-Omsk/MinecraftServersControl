using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum VideoType
    {
        Video,
        MusicVideo,
        Movie
    }
}
