using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum ButtonActionType
    {
        Text,
        OpenLink,
        Location,
        Vkpay,
        OpenApp,
        Callback
    }
}
