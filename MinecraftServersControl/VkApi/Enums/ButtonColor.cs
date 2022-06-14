using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum ButtonColor
    {
        Primary,
        Secondary,
        Negative,
        Positive
    }
}
