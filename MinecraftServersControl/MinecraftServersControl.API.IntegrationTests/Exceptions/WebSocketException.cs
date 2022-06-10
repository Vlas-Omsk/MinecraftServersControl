using MinecraftServersControl.API.Schema;
using System;

namespace MinecraftServersControl.API.IntegrationTests
{
    public sealed class WebSocketException : Exception
    {
        public WebSocketResponse Response { get; }

        public WebSocketException(WebSocketResponse response) : base($"{response.Code}: {response.Code}")
        {
            Response = response;
        }
    }
}
