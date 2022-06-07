using MinecraftServersControl.API.Schema;
using System;

namespace MinecraftServersControl.API.IntegrationTests
{
    public sealed class WebSocketException : Exception
    {
        public WebSocketResponseBase Response { get; }

        public WebSocketException(WebSocketResponseBase response) : base($"{response.Code}: {response.Code}")
        {
            Response = response;
        }
    }
}
