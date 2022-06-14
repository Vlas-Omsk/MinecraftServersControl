using MinecraftServersControl.Common;
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

        public async Task SendResultCode(ResultCode code)
        {
            await Send(Description.GetEnumValueDescription(code) ?? code.ToString());
        }

        public async Task Send(string message)
        {
            await _vkClient.Messages.Send(peerId: _message.PeerId, message: message, randomId: 0);
        }
    }
}
