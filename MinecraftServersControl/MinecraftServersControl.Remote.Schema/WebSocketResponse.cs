using MinecraftServersControl.Remote.DTO;
using System;

namespace MinecraftServersControl.Remote.Schema
{
    [Serializable]
    public class WebSocketResponse<T> : WebSocketResponse where T : Result
    {
        public T Result { get; protected set; }

        protected WebSocketResponse()
        {
        }

        public WebSocketResponse(int requestId, T result) : base(requestId)
        {
            Result = result;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId}, Result: {Result})";
        }
    }

    [Serializable]
    public class WebSocketResponse
    {
        public int RequestId { get; protected set; }

        protected WebSocketResponse()
        {
        }

        public WebSocketResponse(int requestId)
        {
            RequestId = requestId;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId})";
        }
    }
}
