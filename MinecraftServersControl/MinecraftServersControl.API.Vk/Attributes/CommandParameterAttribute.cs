using System;

namespace MinecraftServersControl.API.Vk
{
    [AttributeUsage(AttributeTargets.Parameter)]
    internal sealed class CommandParameterAttribute : Attribute
    {
        public string Name { get; }
        public string Description { get; set; }

        public CommandParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
