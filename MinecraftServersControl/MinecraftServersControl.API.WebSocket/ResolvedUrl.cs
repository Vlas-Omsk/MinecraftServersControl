using System;
using System.Reflection;

namespace MinecraftServersControl.API
{
    internal class ResolvedUrl
    {
        public Type ServiceType { get; }
        public MethodInfo Method { get; }
        public string[] CurrentSegments { get; }
        public UrlSegment[] TargetSegments { get; }

        public ResolvedUrl(Type serviceType, MethodInfo method, string[] currentSegments, UrlSegment[] targetSegments)
        {
            ServiceType = serviceType;
            Method = method;
            CurrentSegments = currentSegments;
            TargetSegments = targetSegments;
        }
    }
}
