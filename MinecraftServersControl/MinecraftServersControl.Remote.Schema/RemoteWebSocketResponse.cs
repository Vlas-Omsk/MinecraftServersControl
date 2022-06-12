using MinecraftServersControl.Remote.DTO;
using System;

namespace MinecraftServersControl.Remote.Schema
{
    [Serializable]
    public class RemoteWebSocketResponse<T> : RemoteWebSocketResponse where T : RemoteResult
    {
        public T Result { get; protected set; }

        protected RemoteWebSocketResponse()
        {
        }

        public RemoteWebSocketResponse(int requestId, T result) : base(requestId)
        {
            Result = result;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId}, Result: {Result})";
        }
    }

    [Serializable]
    public class RemoteWebSocketResponse
    {
        public int RequestId { get; protected set; }

        protected RemoteWebSocketResponse()
        {
        }

        public RemoteWebSocketResponse(int requestId)
        {
            RequestId = requestId;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId})";
        }
    }
}
