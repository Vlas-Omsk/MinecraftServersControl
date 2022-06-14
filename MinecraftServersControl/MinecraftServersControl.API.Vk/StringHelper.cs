using System;

namespace MinecraftServersControl.API.Vk
{
    public static class StringHelper
    {
        public static string ToString(bool value)
        {
            return value ? "да" : "нет";
        }
    }
}
