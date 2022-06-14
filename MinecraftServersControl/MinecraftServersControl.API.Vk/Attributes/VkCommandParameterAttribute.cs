using System;

namespace MinecraftServersControl.API.Vk
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class VkCommandParameterAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; set; }

        public VkCommandParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
