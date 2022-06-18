using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.API.Schema
{
    public class HttpResponse<T> : HttpResponse
    {
        public T Data { get; protected set; }

        protected HttpResponse()
        {
        }

        public HttpResponse(string errorMessage) : base(errorMessage)
        {
        }

        public HttpResponse(string errorMessage, ErrorCode errorCode) : base(errorMessage, errorCode)
        {
        }

        public HttpResponse(T data)
        {
            Data = data;
        }

        public override string ToString()
        {
            return $"(HasErrors: {HasErrors}, Data: {Data})";
        }
    }

    public class HttpResponse
    {
        public bool HasErrors { get; protected set; }
        public ErrorCode ErrorCode { get; protected set; }
        public string ErrorMessage { get; protected set; }

        protected HttpResponse()
        {
        }

        public HttpResponse(string errorMessage)
        {
            HasErrors = true;
            ErrorMessage = errorMessage;
        }

        public HttpResponse(string errorMessage, ErrorCode errorCode)
        {
            HasErrors = true;
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public override string ToString()
        {
            return $"(HasErrors: {HasErrors})";
        }
    }
}
