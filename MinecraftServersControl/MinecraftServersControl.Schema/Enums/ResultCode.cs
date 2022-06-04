using System;

namespace MinecraftServersControl.Core.DTO
{
    public enum ResultCode : int
    {
        Success = 0,

        UserNotFound = 1,
        SessionExpired = 2,
        AuthorizationFromAnotherPlace = 3,
    }
}
