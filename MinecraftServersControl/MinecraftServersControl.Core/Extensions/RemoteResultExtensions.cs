using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Remote.DTO;
using System;

namespace MinecraftServersControl.Core
{
    public static class RemoteResultExtensions
    {
        public static void ThrowOnError(this RemoteResult result)
        {
            switch (result.Code)
            {
                case RemoteResultCode.Success:
                    return;
                case RemoteResultCode.ServerStarted:
                    throw new CoreException(ErrorCode.ServerStarted);
                case RemoteResultCode.ServerStopped:
                    throw new CoreException(ErrorCode.ServerStopped);
                case RemoteResultCode.ServerNotFound:
                    throw new CoreException(ErrorCode.ServerNotFound);
                case RemoteResultCode.CantStartServer:
                    throw new CoreException(ErrorCode.CantStartServer);
                case RemoteResultCode.ServerOutput:
                    throw new CoreException(ErrorCode.ServerOutput);
                default:
                    throw new NotImplementedException("The remote computer returned an invalid response");
            }
        }
    }
}
