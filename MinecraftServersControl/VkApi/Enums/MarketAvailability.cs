using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(int))]
    public enum MarketAvailability : int
    {
        Available = 0,
        Deleted = 1,
        Unavailable = 2,
    }
}
