using System;

namespace VkApi
{
    [EnumDeserializerType(typeof(string))]
    public enum TemplateElementActionType
    {
        OpenLink,
        OpenPhoto
    }
}
