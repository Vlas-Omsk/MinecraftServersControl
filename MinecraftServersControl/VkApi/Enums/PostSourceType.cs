using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum PostSourceType
    {
        Vk,
        Widget,
        Api,
        Rss,
        Sms
    }
}
