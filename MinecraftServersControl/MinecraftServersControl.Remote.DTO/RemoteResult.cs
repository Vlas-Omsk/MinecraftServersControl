using System;
using System.Collections.Generic;

namespace MinecraftServersControl.Remote.DTO
{
    [Serializable]
    public class RemoteResult<T> : RemoteResult
    {
        public T Data { get; protected set; }

        protected RemoteResult()
        {
        }

        public RemoteResult(T data) : this(RemoteResultCode.Success)
        {
            Data = data;
        }

        public RemoteResult(T data, RemoteResultCode code) : this(code)
        {
            Data = data;
        }

        public RemoteResult(T data, RemoteResultCode code, string errorMessage) : this(code, errorMessage)
        {
            Data = data;
        }

        public RemoteResult(RemoteResultCode code) : base(code)
        {
        }

        public RemoteResult(RemoteResultCode code, string errorMessage) : base(code, errorMessage)
        {
        }

        public override string ToString()
        {
            return $"(Code: {Code}, Data: {(EqualityComparer<T>.Default.Equals(Data, default) ? "null" : Data)})";
        }

        public static implicit operator RemoteResult<T>(RemoteResultCode code) => new RemoteResult<T>(code);
        public static implicit operator RemoteResult<T>(T data) => new RemoteResult<T>(data);
    }

    [Serializable]
    public class RemoteResult
    {
        public RemoteResultCode Code { get; protected set; }
        public string ErrorMessage { get; protected set; }

        protected RemoteResult()
        {
        }

        public RemoteResult(RemoteResultCode code)
        {
            Code = code;
        }

        public RemoteResult(RemoteResultCode code, string errorMessage)
        {
            Code = code;
            ErrorMessage = errorMessage;
        }

        public RemoteResult<T> ToResult<T>()
        {
            return new RemoteResult<T>(default, Code, ErrorMessage);
        }

        public bool HasErrors()
        {
            return Code != RemoteResultCode.Success;
        }

        public override string ToString()
        {
            return $"(Code: {Code})";
        }

        public static implicit operator RemoteResult(RemoteResultCode code) => new RemoteResult(code);
    }
}
