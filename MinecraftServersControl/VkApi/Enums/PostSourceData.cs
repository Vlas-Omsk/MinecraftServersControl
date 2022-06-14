using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum PostSourceData
    {
        ProfileActivity,
        ProfilePhoto,
        Comments,
        Like,
        Poll
    }
}
