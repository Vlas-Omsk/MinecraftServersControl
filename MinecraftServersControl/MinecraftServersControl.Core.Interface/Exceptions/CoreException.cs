using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.Core.Abstractions
{
    public sealed class CoreException : Exception
    {
        public ErrorCode ErrorCode { get; }
        public string ErrorMessage { get; }

        public CoreException(ErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }

        public CoreException(ErrorCode errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}
