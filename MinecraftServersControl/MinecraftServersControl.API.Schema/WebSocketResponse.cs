using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public class WebSocketResponse<T> : WebSocketResponse
    {
        public T Data { get; protected set; }

        protected WebSocketResponse()
        {
        }

        public WebSocketResponse(int requestId, WebSocketResponseCode code, string errorMessage, T data) :
            base(requestId, code, errorMessage)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId}, Code: {Code}, Data: {Data})";
        }
    }

    [Serializable]
    public class WebSocketResponse
    {
        public int RequestId { get; protected set; }
        public WebSocketResponseCode Code { get; protected set; }
        public ErrorCode ErrorCode { get; protected set; }
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

        public WebSocketResponse(int requestId, WebSocketResponseCode code, ErrorCode errorCode, string errorMessage)
        {
            RequestId = requestId;
            Code = code;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId}, Code: {Code})";
        }
    }
}
