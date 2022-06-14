using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum DonutEditMode
    {
        All,
        Duration
    }
}
