using System;
using System.Threading.Tasks;
using VkApi.Models;

namespace MinecraftServersControl.API.Vk
{
    public delegate Task MessageHandlerOverride(Message message);

    public sealed class VkSession
    {
        public VkSessionState State { get; set; }
        public MessageHandlerOverride HandlerOverride { get; set; }
    }
}
