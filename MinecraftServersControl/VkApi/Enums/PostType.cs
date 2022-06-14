using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum PostType
    {
        Post,
        Copy,
        Reply,
        Postpone,
        Suggest
    }
}
