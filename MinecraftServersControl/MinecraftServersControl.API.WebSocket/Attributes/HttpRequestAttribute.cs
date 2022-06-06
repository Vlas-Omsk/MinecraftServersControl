using System;

namespace MinecraftServersControl.API.WebSocket
{
    [AttributeUsage(AttributeTargets.Method)]
    internal sealed class HttpRequest : Attribute
    {
        public HttpMethod HttpMethod { get; }
        public string Path { get; }

        public HttpRequest(HttpMethod httpMethod, string path)
        {
            HttpMethod = httpMethod;
            Path = path;
        }
    }
}
