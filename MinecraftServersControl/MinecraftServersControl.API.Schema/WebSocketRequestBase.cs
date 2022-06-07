using PinkJson2;
using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public sealed class WebSocketRequestBase
    {
        public int Id { get; private set; }
        public WebSocketRequestCode Code { get; private set; }
        public IJson Data { get; private set; }

        private WebSocketRequestBase()
        {
        }
    }
}
