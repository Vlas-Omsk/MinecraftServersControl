using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public class HttpResponse<T> : HttpResponse where T : Result
    {
        public T Result { get; protected set; }

        protected HttpResponse()
        {
        }

        public HttpResponse(string errorMessage) : base(errorMessage)
        {
        }

        public HttpResponse(T result)
        {
            Result = result;
        }

        public override string ToString()
        {
            return $"(HasErrors: {HasErrors}, Result: {Result})";
        }
    }

    [Serializable]
    public class HttpResponse
    {
        public bool HasErrors { get; protected set; }
        public string ErrorMessage { get; protected set; }

        protected HttpResponse()
        {
        }

        public HttpResponse(string errorMessage)
        {
            HasErrors = true;
            ErrorMessage = errorMessage;
        }

        public override string ToString()
        {
            return $"(HasErrors: {HasErrors})";
        }
    }
}
