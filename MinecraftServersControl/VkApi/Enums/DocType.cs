using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(int))]
    public enum DocType : int
    {
        TextDocument = 1,
        Archive = 2,
        Gif = 3,
        Image = 4,
        Audio = 5,
        Video = 6,
        ElectronicBook = 7,
        Other = 8
    }
}
