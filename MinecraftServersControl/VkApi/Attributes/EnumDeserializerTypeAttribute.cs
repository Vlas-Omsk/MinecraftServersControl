using System;

namespace VkApi
{
    [AttributeUsage(AttributeTargets.Enum)]
    public sealed class EnumDeserializerTypeAttribute : Attribute
    {
        public Type Type { get; }

        public EnumDeserializerTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}
