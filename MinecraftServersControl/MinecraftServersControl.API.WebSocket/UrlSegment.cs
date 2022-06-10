using System;

namespace MinecraftServersControl.API
{
    internal class UrlSegment
    {
        public string Name { get; }
        public bool IsParameter { get; }

        public UrlSegment(string segment)
        {
            IsParameter = segment.StartsWith(':');
            Name = !IsParameter ?
                segment :
                segment.Substring(1, segment.Length - 1);
        }
    }
}
