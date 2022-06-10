using PinkJson2;
using PinkJson2.Serializers;
using System;

namespace MinecraftServersControl.Common
{
    public static class JsonExtensions
    {
        public static bool TryParseJson(this string self, out IJson json, out Exception exception)
        {
            try
            {
                json = Json.Parse(self);
                exception = null;
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                json = null;
                return false;
            }
        }

        public static bool TryDeserialize<T>(this IJson self, out T obj, out Exception exception)
        {
            var result = TryDeserialize(self, typeof(T), out object objSource, out exception);
            obj = (T)objSource;
            return result;
        }

        public static bool TryDeserialize(this IJson self, Type type, out object obj, out Exception exception)
        {
            try
            {
                obj = self.Deserialize(type, new ObjectSerializerOptions()
                {
                    IgnoreMissingProperties = false
                });
                exception = null;
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                obj = null;
                return false;
            }
        }
    }
}
