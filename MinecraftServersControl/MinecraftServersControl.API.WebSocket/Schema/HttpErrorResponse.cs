using System;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public sealed class HttpErrorResponse
    {
        public string ErrorMessage { get; }

        public HttpErrorResponse(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
