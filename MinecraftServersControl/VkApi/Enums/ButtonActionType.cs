using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum ButtonActionType
    {
        OpenUrl,
        Text,
        OpenLink,
        Location,
        Vkpay,
        OpenApp,
        Callback,
        OpenPhoto
    }
}
