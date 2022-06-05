using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    public sealed class WebSocketResponse : Response
    {
        public int RequestId { get; set; }

        public const int BroadcastRequestId = -1;

        public WebSocketResponse(int requestId, ResponseCode code, string errorMessage, Result result) : base(code, errorMessage, result)
        {
            RequestId = requestId;
        }

        public WebSocketResponse(int requestId, Response response) : base(response.Code, response.ErrorMessage, response.Result)
        {
            RequestId = requestId;
        }
    }
}
