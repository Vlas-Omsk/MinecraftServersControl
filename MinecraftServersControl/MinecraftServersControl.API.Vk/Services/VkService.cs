using System;

namespace MinecraftServersControl.API.Vk.Services
{
    public abstract class VkService
    {
        protected VkMessageHandler Handler { get; private set; }

        internal VkService()
        {
        }

        internal void Init(VkMessageHandler handler)
        {
            Handler = handler;
        }
    }
}
