using PinkJson2;
using PinkJson2.Serializers;
using System;

namespace MinecraftServersControl.Common
{
    public static class JsonExtensions
    {
        public static T DeserializeCustom<T>(this IJson self)
        {
            return (T)DeserializeCustom(self, typeof(T));
        }

        public static object DeserializeCustom(this IJson self, Type type)
        {
            return self.Deserialize(type, new ObjectSerializerOptions()
            {
                IgnoreMissingProperties = false
            });
        }
    }
}
