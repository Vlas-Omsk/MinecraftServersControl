using System;
using System.ComponentModel;

namespace MinecraftServersControl.Core.DTO
{
    public enum ResultCode : int
    {
        [Description("Успешно")]
        Success = 0,

        [Description("Пользователь не найден")]
        UserNotFound = 1,
        SessionExpired = 2,
        AuthorizationFromAnotherPlace = 3,
        [Description("Компьютер не найден")]
        ComputerNotFound = 4,
        [Description("Сервер запущен")]
        ServerStarted = 5,
        [Description("Сервер остановлен")]
        ServerStopped = 6,
        [Description("Компьютер запущен")]
        ComputerStarted = 7,
        [Description("Компьютер отключен")]
        ComputerStopped = 8,
        ServerOutput = 9,
        [Description("Сервер не найден")]
        ServerNotFound = 10,
        [Description("Не удалось запустить сервер")]
        CantStartServer = 11,
    }
}
