using System;

namespace MinecraftServersControl.API.Vk
{
    internal sealed class ServiceAttribute : Attribute
    {
        public string[] Segments { get; }

        public ServiceAttribute(string path)
        {
            Segments = ServiceHelper.ParseSegments(path);
        }
    }
}
