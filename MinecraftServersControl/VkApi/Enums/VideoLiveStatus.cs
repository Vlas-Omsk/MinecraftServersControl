using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum VideoLiveStatus
    {
        Waiting,
        Started,
        Finished,
        Failed,
        Upcoming
    }
}
