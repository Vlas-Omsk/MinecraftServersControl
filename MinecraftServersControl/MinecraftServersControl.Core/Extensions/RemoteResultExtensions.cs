using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Abstractions;
using System;
using MinecraftServersControl.Remote.Server.Schema;
using MinecraftServersControl.Remote.Core.DTO;

namespace MinecraftServersControl.Core
{
    internal static class RemoteResultExtensions
    {
        public static void ThrowOnError(this RemoteWebSocketResponse response)
        {
            switch (response.ErrorCode)
            {
                case RemoteErrorCode.None:
                    return;
                case RemoteErrorCode.ServerAlredyStarted:
                    throw new CoreException(ErrorCode.ServerAlredyStarted);
                case RemoteErrorCode.ServerAlredyStopped:
                    throw new CoreException(ErrorCode.ServeAlredyrStopped);
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
