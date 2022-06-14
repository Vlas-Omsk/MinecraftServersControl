using System;

namespace MinecraftServersControl.API.Vk.VkServices
{
    public abstract class VkService
    {
        protected VkMessageHandler Handler { get; private set; }

        protected VkService()
        {
        }

        internal void Init(VkMessageHandler handler)
        {
            Handler = handler;
        }
    }
}
