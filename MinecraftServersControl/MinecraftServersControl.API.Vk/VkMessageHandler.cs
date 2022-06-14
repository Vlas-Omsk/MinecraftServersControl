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
        public Message Message { get; }
        public VkClient VkClient { get; }
        public IApplication Application { get; }
        public Logger Logger { get; }
        public Type[] ServiceTypes { get; }
        public VkSessionStorage SessionStorage { get; }
        public CommandAccessVerifier CommandAccessVerifier { get; }
        public MessageResponse MessageResponse { get; }
        public VkSession Session { get; }

        public VkMessageHandler(Type[] serviceTypes, Message message, VkSessionStorage sessionStorage, VkClient vkClient, IApplication application, Logger logger)
        {
            ServiceTypes = serviceTypes;
            Message = message;
            SessionStorage = sessionStorage;
            VkClient = vkClient;
            Application = application;
            Logger = logger;
            MessageResponse = new MessageResponse(message, vkClient);
            CommandAccessVerifier = new CommandAccessVerifier(application, message);
            Session = SessionStorage.GetOrCreate(Message.FromId);
        }

        public async Task ProcessMessage()
        {
            Logger.Info($"PeerId: {Message.PeerId}, FromId: {Message.FromId}, Message: {Message.Text}");

            if (string.IsNullOrEmpty(Message.Text))
                return;

            if (Session.HandlerOverride != null)
            {
                try
                {
                    await Session.HandlerOverride.Invoke(Message);
                }
                catch (Exception ex)
                {
                    await SendInternalServerError(ex);
                    Logger.Error(ex.ToString());
                }

                return;
            }

            var result = ServiceHelper.GetCommandFromString(ServiceTypes, ServiceHelper.ParseSegments(Message.Text));

            if (result.Method == null)
            {
                await SendCommandNotFound(result.Matches);
                return;
            }

            var verifyResult = await CommandAccessVerifier.Verify(result.Method);

            if (verifyResult != null)
            {
                await MessageResponse.Send(verifyResult.Message);
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

                obj.Init(this);

                var methodResult = result.Method.Invoke(obj, parameters);

                if (methodResult is Task task)
                    await task;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
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
            if (Message.IsFromChat())
                return;

            await Send($"Ошибка в параметре [{FormatHelper.ToStringParameterShort(parameterException.Parameter)}]: {parameterException.InnerException.Message}");
        }

        private async Task SendCommandNotFound(MethodInfo[] possibleMatches)
        {
            if (Message.IsFromChat())
                return;

            string result = "Команда не распознана";

            if (possibleMatches.Length > 0)
            {
                result += "\r\nВозможные совпадения:";

                foreach (var method in possibleMatches)
                    if (await CommandAccessVerifier.Verify(method) == null)
                        result += "\r\n" + FormatHelper.ToStringCommandShort(method);
            }

            result += "\r\nДля отображения всех команд - 'команды'";

            await Send(result);
        }

        private async Task Send(string message)
        {
            await VkClient.Messages.Send(peerId: Message.PeerId, message: message, randomId: 0);
        }
    }
}
