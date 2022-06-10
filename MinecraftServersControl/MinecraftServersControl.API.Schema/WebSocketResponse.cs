using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public class WebSocketResponse<T> : WebSocketResponse where T : Result
    {
        public T Result { get; protected set; }

        protected WebSocketResponse()
        {
        }

        public WebSocketResponse(int requestId, WebSocketResponseCode code, string errorMessage, T result) :
            base(requestId, code, errorMessage)
        {
            Result = result;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId}, Code: {Code}, Result: {Result})";
        }
    }

    [Serializable]
    public class WebSocketResponse
    {
        public int RequestId { get; protected set; }
        public WebSocketResponseCode Code { get; protected set; }
        public string ErrorMessage { get; protected set; }

        public const int BroadcastRequestId = -1;

        protected WebSocketResponse()
        {
        }

        public WebSocketResponse(int requestId, WebSocketResponseCode code, string errorMessage)
        {
            RequestId = requestId;
            Code = code;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId}, Code: {Code})";
        }
    }
}
