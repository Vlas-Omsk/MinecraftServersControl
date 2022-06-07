using System;

namespace MinecraftServersControl.Core.DTO
{
    public interface IResult
    {
        ResultCode Code { get; }
        string ErrorMessage { get; }
        object Data { get; }
    }

    [Serializable]
    public sealed class Result<T> : IResult
    {
        public ResultCode Code { get; private set; }
        public string ErrorMessage { get; private set; }
        public T Data { get; private set; }

        private Result()
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

        public Result(ResultCode code)
        {
            Code = code;
        }

        public Result(ResultCode code, string errorMessage)
        {
            Code = code;
            ErrorMessage = errorMessage;
        }

        object IResult.Data => Data;

        public bool HasErrors => Code != ResultCode.Success;

        public static implicit operator Result<T>(T data) => new Result<T>(data);
        public static implicit operator Result<T>(ResultCode code) => new Result<T>(code);
        public static implicit operator Result<T>(Result<object> result) => new Result<T>((T)result.Data, result.Code, result.ErrorMessage);
    }
}
