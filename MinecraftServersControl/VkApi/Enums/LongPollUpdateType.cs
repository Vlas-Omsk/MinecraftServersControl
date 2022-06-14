using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum LongPollUpdateType
    {
        Other,
        MessageNew
    }
}
