using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public sealed class HttpResponse<T>
    {
        public bool HasErrors { get; private set; }
        public string ErrorMessage { get; private set; }
        public Result<T> Result { get; private set; }

        private HttpResponse()
        {
        }

        public HttpResponse(string errorMessage)
        {
            HasErrors = true;
            ErrorMessage = errorMessage;
        }

        public HttpResponse(Result<T> result)
        {
            Result = result;
        }
    }
}
