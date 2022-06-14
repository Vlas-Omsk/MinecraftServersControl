using System;

namespace MinecraftServersControl.API.Vk
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class VkCommandAttribute : Attribute
    {
        public string[] Segments { get; }
        public string Description { get; }

        public VkCommandAttribute(string path)
        {
            Segments = ServiceHelper.ParseSegments(path);
        }
    }
}
