using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(int))]
    public enum Lang
    {
        Russian = 0,
        Ukrainian = 1,
        Belorussian = 2,
        English = 3,
        Spanish = 4,
        Finnish = 5,
        Deutsch = 6,
        Italian = 7
    }
}
