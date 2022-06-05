using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public sealed class WebSocketRequest : Request
    {
        public int Id { get; set; }

        public WebSocketRequest() : base()
        {
        }

        public WebSocketRequest(int id, RequestCode code, object data) : base(code, data)
        {
            Id = id;
        }
    }
}
