using PinkJson2;
using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public sealed class WebSocketResponseBase
    {
        public int RequestId { get; private set; }
        public WebSocketResponseCode Code { get; private set; }
        public string ErrorMessage { get; private set; }
        public IJson Result { get; private set; }

        private WebSocketResponseBase()
        {
        }
    }
}
