using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public class WebSocketRequest<T> : IWebSocketRequest
    {
        public int Id { get; }
        public WebSocketRequestCode Code { get; }
        public T Data { get; }

        public WebSocketRequest(int id, WebSocketRequestCode code, T data)
        {
            Id = id;
            Code = code;
            Data = data;
        }

        object IWebSocketRequest.Data => Data;
    }
}
