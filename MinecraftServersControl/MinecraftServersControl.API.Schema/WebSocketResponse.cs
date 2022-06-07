using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    public static class WebSocketResponse
    {
        public const int BroadcastRequestId = -1;
    }

    [Serializable]
    public class WebSocketResponse<T> : IWebSocketResponse
    {
        public int RequestId { get; }
        public WebSocketResponseCode Code { get; }
        public string ErrorMessage { get; }
        public Result<T> Result { get; }

        public WebSocketResponse(int requestId, WebSocketResponseCode code, string errorMessage, Result<T> result)
        {
            RequestId = requestId;
            Code = code;
            ErrorMessage = errorMessage;
            Result = result;
        }

        IResult IWebSocketResponse.Result => Result;
    }
}
