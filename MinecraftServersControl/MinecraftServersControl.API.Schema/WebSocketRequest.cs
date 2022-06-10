using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public class WebSocketRequest<T> : WebSocketRequest
    {
        public T Data { get; protected set; }

        protected WebSocketRequest()
        {
        }

        public WebSocketRequest(int id, WebSocketRequestCode code, T data) : base(id, code)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"(Id: {Id}, Code: {Code}, Data: {Data})";
        }
    }

    [Serializable]
    public class WebSocketRequest
    {
        public int Id { get; protected set; }
        public WebSocketRequestCode Code { get; protected set; }

        protected WebSocketRequest()
        {
        }

        public WebSocketRequest(int id, WebSocketRequestCode code)
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
