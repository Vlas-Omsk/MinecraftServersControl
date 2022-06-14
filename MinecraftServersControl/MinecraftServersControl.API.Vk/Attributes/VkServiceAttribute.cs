using System;

namespace MinecraftServersControl.API.Vk
{
    public sealed class VkServiceAttribute : Attribute
    {
        public string[] Segments { get; }

        public VkServiceAttribute(string path)
        {
            Segments = ServiceHelper.ParseSegments(path);
        }
    }
}
