using MinecraftServersControl.Remote.DTO;
using System;

namespace MinecraftServersControl.Remote.Core
{
    public sealed class RemoteCoreException : Exception
    {
        public RemoteErrorCode ErrorCode { get; }

        public RemoteCoreException(RemoteErrorCode errorCode)
        {
            ErrorCode = errorCode;
        }
    }
}
