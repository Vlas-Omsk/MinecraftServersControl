using System;
using VkApi.Models;

namespace MinecraftServersControl.API.Vk
{
    internal static class MessageExtensions
    {
        public static bool IsFromChat(this Message self)
        {
            return self.PeerId - 2000000000 > 0;
        }
    }
}
