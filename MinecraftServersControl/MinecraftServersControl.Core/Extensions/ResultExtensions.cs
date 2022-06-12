using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Remote.DTO;
using System;
using System.Linq;

namespace MinecraftServersControl.Core
{
    public static class ResultExtensions
    {
        public static Result FromRemoteResult(this RemoteResult result, params RemoteResultCode[] availableCodes)
        {
            if (!availableCodes.Any(x => result.Code == x))
                throw new ArgumentOutOfRangeException("The remote computer returned an invalid response");

            return new Result(GetCode(result.Code), result.ErrorMessage);
        }

        public static Result<T> FromRemoteResult<T>(this RemoteResult<T> result, params RemoteResultCode[] availableCodes)
        {
            if (!availableCodes.Any(x => result.Code == x))
                throw new ArgumentOutOfRangeException("The remote computer returned an invalid response");

            return new Result<T>(result.Data, GetCode(result.Code), result.ErrorMessage);
        }

        private static ResultCode GetCode(RemoteResultCode code)
        {
            return code switch
            {
                RemoteResultCode.Success => ResultCode.Success,
                RemoteResultCode.ServerStarted => ResultCode.ServerStarted,
                RemoteResultCode.ServerStopped => ResultCode.ServerStopped,
                RemoteResultCode.ServerNotFound => ResultCode.ServerNotFound,
                RemoteResultCode.CantStartServer => ResultCode.CantStartServer,
                RemoteResultCode.ServerOutput => ResultCode.ServerOutput,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
