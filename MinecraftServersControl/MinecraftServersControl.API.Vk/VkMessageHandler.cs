using MinecraftServersControl.API.Vk.VkServices;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;
using VkApi;
using VkApi.Models;

namespace MinecraftServersControl.API.Vk
{
    public sealed class VkMessageHandler
    {
        private readonly Message _message;
        private readonly VkClient _vkClient;
        private readonly IApplication _application;
        private readonly Logger _logger;
        private readonly Type[] _serviceTypes;
        private readonly VkSessionStorage _sessionStorage;

        public VkMessageHandler(Type[] serviceTypes, Message message, VkSessionStorage sessionStorage, VkClient vkClient, IApplication application, Logger logger)
        {
            _serviceTypes = serviceTypes;
            _message = message;
            _sessionStorage = sessionStorage;
            _vkClient = vkClient;
            _application = application;
            _logger = logger;
        }

        public async Task ProcessMessage()
        {
            _logger.Info($"PeerId: {_message.PeerId}, FromId: {_message.FromId}, Message: {_message.Text}");

            var session = _sessionStorage.GetOrCreate(_message.FromId);

            if (session.HandlerOverride != null)
            {
                try
                {
                    await session.HandlerOverride.Invoke(_message);
                }
                catch (Exception ex)
                {
                    await SendInternalServerError(ex);
                    _logger.Error(ex.ToString());
                }

                return;
            }

            var result = ServiceHelper.GetCommandFromString(_serviceTypes, ServiceHelper.ParseSegments(_message.Text));

            if (result.Method == null)
            {
                await SendCommandNotFound(result.Matches);
                return;
            }

            object[] parameters;

            try
            {
                parameters = ServiceHelper.MapParameters(result.Method, result.ParametersSegments);
            }
            catch (ParameterException ex)
            {
                await SendParameterError(ex);
                return;
            }

            try
            {
                var objCctor = result.Method.DeclaringType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, Type.EmptyTypes, null);
                var obj = (VkService)objCctor.Invoke(null);

                obj.Init(_application, _logger, _message, _vkClient, session, _serviceTypes);

                var methodResult = result.Method.Invoke(obj, parameters);

                if (methodResult is Task task)
                    await task;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                await SendInternalServerError(ex);
                return;
            }
        }

        private async Task SendInternalServerError(Exception ex)
        {
            await Send($"Внутренняя ошибка сервера");
        }

        private async Task SendParameterError(ParameterException parameterException)
        {
            if (_message.IsFromChat())
                return;

            await Send($"Ошибка в параметре [{ServiceHelper.FormatParameterShort(parameterException.Parameter)}]: {parameterException.InnerException.Message}");
        }

        private async Task SendCommandNotFound(MethodInfo[] possibleMatches)
        {
            if (_message.IsFromChat())
                return;

            string result = "Команда не распознана";

            if (possibleMatches.Length > 0)
            {
                result += "\r\nВозможные совпадения:";

                foreach (var method in possibleMatches)
                    result += "\r\n" + ServiceHelper.FormatCommandShort(method);
            }

            result += "\r\nДля отображения всех команд - 'команды'";

            await Send(result);
        }

        private async Task Send(string message)
        {
            await _vkClient.Messages.Send(peerId: _message.PeerId, message: message, randomId: 0);
        }
    }
}
