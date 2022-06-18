using System;

namespace MinecraftServersControl.Remote.Server.Schema
{
    public class RemoteWebSocketRequest<T> : RemoteWebSocketRequest
    {
        public T Data { get; protected set; }

        protected RemoteWebSocketRequest()
        {
        }

        public RemoteWebSocketRequest(int id, RemoteWebSocketRequestCode code, T data) : base(id, code)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"(Id: {Id}, Code: {Code}, Data: {Data})";
        }
    }

    public class RemoteWebSocketRequest
    {
        public int Id { get; protected set; }
        public RemoteWebSocketRequestCode Code { get; protected set; }

        protected RemoteWebSocketRequest()
        {
        }

        public RemoteWebSocketRequest(int id, RemoteWebSocketRequestCode code)
        {
            Id = id;
            Code = code;
        }

        public override string ToString()
        {
            return $"(Id: {Id}, Code: {Code})";
        }
    }
}
