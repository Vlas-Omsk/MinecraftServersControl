using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public class WebSocketResponse
    {
        public int RequestId { get; }
        public WebSocketResponseCode Code { get; }
        public string ErrorMessage { get; }
        public Result Result { get; }

        public const int BroadcastRequestId = -1;

        public WebSocketResponse(int requestId, WebSocketResponseCode code, string errorMessage, Result result)
        {
            RequestId = requestId;
            Code = code;
            ErrorMessage = errorMessage;
            Result = result;
        }

        public static WebSocketResponse CreateError(int requestId, WebSocketResponseCode code, string message)
        {
            return new WebSocketResponse(requestId, code, message, null);
        }

        public static WebSocketResponse CreateSuccess(int requestId, Result result)
        {
            return new WebSocketResponse(requestId, WebSocketResponseCode.Success, null, result);
        }
    }
}
