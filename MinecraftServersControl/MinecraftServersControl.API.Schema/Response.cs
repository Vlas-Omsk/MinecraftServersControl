using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public sealed class Response
    {
        public int RequestId { get; }
        public ResponseCode Code { get; }
        public string ErrorMessage { get; }
        public Result Result { get; }

        public const int BroadcastRequestId = -1;

        public Response(int requestId, ResponseCode code, string errorMessage, Result result)
        {
            RequestId = requestId;
            Code = code;
            ErrorMessage = errorMessage;
            Result = result;
        }
    }
}
