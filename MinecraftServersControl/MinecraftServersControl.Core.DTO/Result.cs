using System;

namespace MinecraftServersControl.Core.DTO
{
    [Serializable]
    public class Result
    {
        public ResultCode Code { get; }
        public string ErrorMessage { get; }

        public Result(ResultCode code)
        {
            Code = code;
        }

        public Result(ResultCode code, string errorMessage)
        {
            Code = code;
            ErrorMessage = errorMessage;
        }

        public bool HasErrors()
        {
            return Code != ResultCode.Success;
        }

        public Result<T> ToGeneric<T>()
        {
            return new Result<T>(Code, ErrorMessage);
        }

        public static implicit operator Result(ResultCode code) => new Result(code);
    }

    [Serializable]
    public sealed class Result<T> : Result
    {
        public T Data { get; }

        public Result(T data) : base(ResultCode.Success)
        {
            Data = data;
        }

        public Result(T data, ResultCode code) : base(code)
        {
            Data = data;
        }

        public Result(T data, ResultCode code, string errorMessage) : base(code, errorMessage)
        {
            Data = data;
        }

        public Result(ResultCode code) : base(code)
        {
        }

        public Result(ResultCode code, string errorMessage) : base(code, errorMessage)
        {
        }

        public static implicit operator Result<T>(T data) => new Result<T>(data);
        public static implicit operator Result<T>(ResultCode code) => new Result<T>(code);
    }
}
