using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum ContentSourceType
    {
        Message,
        Url
    }
}
