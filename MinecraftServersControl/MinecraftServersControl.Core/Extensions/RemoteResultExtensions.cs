using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Schema;
using System;

namespace MinecraftServersControl.Core
{
    public static class RemoteResultExtensions
    {
        public static void ThrowOnError(this RemoteWebSocketResponse response)
        {
            switch (response.ErrorCode)
            {
                case RemoteErrorCode.None:
                    return;
                case RemoteErrorCode.ServerStarted:
                    throw new CoreException(ErrorCode.ServerStarted);
                case RemoteErrorCode.ServerStopped:
                    throw new CoreException(ErrorCode.ServerStopped);
                case RemoteErrorCode.ServerNotFound:
                    throw new CoreException(ErrorCode.ServerNotFound);
                case RemoteErrorCode.CantStartServer:
                    throw new CoreException(ErrorCode.CantStartServer);
                default:
                    throw new NotImplementedException("The remote computer returned an invalid response");
            }
        }
    }
}
