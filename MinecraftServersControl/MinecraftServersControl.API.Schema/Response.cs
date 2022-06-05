using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public class Response
    {
        public ResponseCode Code { get; }
        public string ErrorMessage { get; }
        public Result Result { get; }

        public Response(ResponseCode code, string errorMessage, Result result)
        {
            Code = code;
            ErrorMessage = errorMessage;
            Result = result;
        }

        public static Response CreateError(ResponseCode code, string message)
        {
            return new Response(code, message, null);
        }

        public static Response CreateSuccess(Result result)
        {
            return new Response(ResponseCode.Success, null, result);
        }
    }
}
