using MinecraftServersControl.Remote.Core.DTO;
using System;

namespace MinecraftServersControl.Remote.Server.Schema
{
    public class RemoteWebSocketResponse<T> : RemoteWebSocketResponse
    {
        public T Data { get; protected set; }

        protected RemoteWebSocketResponse()
        {
        }

        public RemoteWebSocketResponse(int requestId, RemoteWebSocketResponseCode code, RemoteErrorCode errorCode, T data) : base(requestId, code, errorCode)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId}, Data: {Data})";
        }
    }

    public class RemoteWebSocketResponse
    {
        public int RequestId { get; protected set; }
        public RemoteWebSocketResponseCode Code { get; protected set; }
        public RemoteErrorCode ErrorCode { get; protected set; }

        protected RemoteWebSocketResponse()
        {
        }

        public RemoteWebSocketResponse(int requestId, RemoteWebSocketResponseCode code, RemoteErrorCode errorCode)
        {
            RequestId = requestId;
            Code = code;
            ErrorCode = errorCode;
        }

        public override string ToString()
        {
            return $"(RequestId: {RequestId})";
        }

        public static RemoteWebSocketResponse<T> CreateBroadcast<T>(RemoteWebSocketResponseCode code, T data)
        {
            return new RemoteWebSocketResponse<T>(-1, code, RemoteErrorCode.None, data);
        }

        public static RemoteWebSocketResponse CreateBroadcast(RemoteWebSocketResponseCode code)
        {
            return new RemoteWebSocketResponse(-1, code, RemoteErrorCode.None);
        }

        public static RemoteWebSocketResponse<T> CreateSuccess<T>(int requestId, T data)
        {
            return new RemoteWebSocketResponse<T>(requestId, RemoteWebSocketResponseCode.Success, RemoteErrorCode.None, data);
        }

        public static RemoteWebSocketResponse CreateSuccess(int requestId)
        {
            return new RemoteWebSocketResponse(requestId, RemoteWebSocketResponseCode.Success, RemoteErrorCode.None);
        }
    }
}
