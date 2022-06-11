using System;
using System.Collections.Generic;

namespace MinecraftServersControl.Remote.DTO
{
    [Serializable]
    public class Result<T> : Result
    {
        public T Data { get; protected set; }

        protected Result()
        {
        }

        public Result(T data) : this(ResultCode.Success)
        {
            Data = data;
        }

        public Result(T data, ResultCode code) : this(code)
        {
            Data = data;
        }

        public Result(T data, ResultCode code, string errorMessage) : this(code, errorMessage)
        {
            Data = data;
        }

        public Result(ResultCode code) : base(code)
        {
        }

        public Result(ResultCode code, string errorMessage) : base(code, errorMessage)
        {
        }

        public override string ToString()
        {
            return $"(Code: {Code}, Data: {(EqualityComparer<T>.Default.Equals(Data, default) ? "null" : Data)})";
        }

        public static implicit operator Result<T>(ResultCode code) => new Result<T>(code);
        public static implicit operator Result<T>(T data) => new Result<T>(data);
    }

    [Serializable]
    public class Result
    {
        public ResultCode Code { get; protected set; }
        public string ErrorMessage { get; protected set; }

        protected Result()
        {
        }

        public Result(ResultCode code)
        {
            Code = code;
        }

        public Result(ResultCode code, string errorMessage)
        {
            Code = code;
            ErrorMessage = errorMessage;
        }

        public Result<T> ToResult<T>()
        {
            return new Result<T>(default, Code, ErrorMessage);
        }

        public bool HasErrors()
        {
            return Code != ResultCode.Success;
        }

        public override string ToString()
        {
            return $"(Code: {Code})";
        }

        public static implicit operator Result(ResultCode code) => new Result(code);
    }
}
