using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;
using VkApi;
using VkApi.Models;

namespace MinecraftServersControl.API.Vk
{
    public sealed class MessageResponse
    {
        private readonly Message _message;
        private readonly VkClient _vkClient;

        public MessageResponse(Message message, VkClient vkClient)
        {
            _message = message;
            _vkClient = vkClient;
        }

        public async Task SendMethodUnavailableFromChat()
        {
            await Send("Команда доступна только в личных сообщениях");
        }

        public async Task SendErrorCode(ErrorCode code)
        {
            var message = code switch
            {
                ErrorCode.UserNotFound => "Пользователь не найден",
                ErrorCode.SessionExpired => "Сессия просрочена",
                ErrorCode.ComputerNotFound => "Компьютер не найден",
                ErrorCode.ServerAlredyStarted => "Сервер уже запущен",
                ErrorCode.ServeAlredyrStopped => "Сервер уже остановлен",
                ErrorCode.ComputerAlredyStarted => "Компьютер уже запущен",
                ErrorCode.ComputerAlredyStopped => "Компьютер уже выключен",
                ErrorCode.ServerNotFound => "Сервер не найден",
                ErrorCode.CantStartServer => "Не удалось запустить сервер",
                _ => code.ToString()
            };

            await Send(message);
        }

        public async Task SendSuccess()
        {
            await Send("Успешно");
        }

        public async Task Send(string message)
        {
            if (string.IsNullOrEmpty(message))
                message = "<Пусто>";

            await _vkClient.Messages.Send(peerId: _message.PeerId, message: message, randomId: 0);
        }
    }
}
