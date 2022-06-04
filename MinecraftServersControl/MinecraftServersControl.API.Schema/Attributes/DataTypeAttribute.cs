using System;

namespace MinecraftServersControl.API.Schema
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class DataTypeAttribute : Attribute
    {
        public Type Type { get; }

        public DataTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}
