using PinkJson2;
using System;

namespace MinecraftServersControl.Common
{
    public static class JsonTypeConversions
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered)
                return;

            _registered = true;

            TypeConverter.Default.Register(typeof(Enum), new TypeConversion((object obj, Type targetType, ref bool handled) =>
            {
                if (obj is int value)
                {
                    handled = true;
                    return Enum.ToObject(targetType, value);
                }

                return null;
            }, (object obj, Type targetType, ref bool handled) =>
            {
                handled = true;
                return Convert.ChangeType(obj, typeof(int));
            }));
        }
    }
}
