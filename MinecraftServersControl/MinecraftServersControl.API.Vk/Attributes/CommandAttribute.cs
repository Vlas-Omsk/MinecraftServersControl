using System;

namespace MinecraftServersControl.API.Vk
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class CommandAttribute : Attribute
    {
        public string[] Segments { get; }

        public CommandAttribute(string path)
        {
            Segments = ServiceHelper.ParseSegments(path);
        }
    }
}
