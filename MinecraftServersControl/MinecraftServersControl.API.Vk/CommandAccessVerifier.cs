using MinecraftServersControl.Core.Interface;
using System;
using System.Reflection;
using System.Threading.Tasks;
using VkApi.Models;

namespace MinecraftServersControl.API.Vk
{
    public sealed class CommandAccessVerifier
    {
        private readonly IApplication _application;
        private readonly Message _message;

        private bool? _isAuthprized;

        public CommandAccessVerifier(IApplication application, Message message)
        {
            _application = application;
            _message = message;
        }

        private async Task<bool> IsAuthorized()
        {
            if (_isAuthprized.HasValue)
                return _isAuthprized.Value;

            return (
                _isAuthprized = await _application.VkUserService.IsAuthorized(_message.FromId)
            ).Value;
        }

        public async Task<VerifyResult> Verify(MethodInfo method)
        {
            if (method.GetCustomAttribute<AuthorizedOnlyAttribute>() != null && !await IsAuthorized())
                return new VerifyResult("Недостаточно прав");

            if (method.GetCustomAttribute<NonChatAttribute>() != null && _message.IsFromChat())
                return new VerifyResult("Команда доступна только в личных сообщениях");

            return null;
        }
    }
}
