using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum VkSizeType
    {
        S,
        M,
        X,
        O,
        P,
        Q,
        R,
        Y,
        Z,
        W
    }
}
