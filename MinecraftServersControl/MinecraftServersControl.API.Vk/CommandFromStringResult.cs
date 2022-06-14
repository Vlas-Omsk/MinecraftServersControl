using System;
using System.Reflection;

namespace MinecraftServersControl.API.Vk
{
    public sealed class CommandFromStringResult
    {
        public MethodInfo Method { get; }
        public string[] ParametersSegments { get; }
        public MethodInfo[] Matches { get; }

        public CommandFromStringResult(MethodInfo method, string[] parametersSegments, MethodInfo[] matches)
        {
            Method = method;
            ParametersSegments = parametersSegments;
            Matches = matches;
        }
    }
}
