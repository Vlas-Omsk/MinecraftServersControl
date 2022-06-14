using MinecraftServersControl.Common;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Logging;
using System;
using System.Threading.Tasks;
using VkApi;
using VkApi.Models;

namespace MinecraftServersControl.API.Vk.VkServices
{
    public abstract class VkService
    {
        protected IApplication Application { get; private set; }
        protected Logger Logger { get; private set; }
        protected Message Message { get; private set; }
        protected VkClient VkClient { get; private set; }
        protected VkSession Session { get; private set; }
        public Type[] ServiceTypes { get; private set; }

        protected VkService()
        {
        }

        internal void Init(IApplication application, Logger logger, Message message, VkClient vkClient, VkSession session, Type[] serviceTypes)
        {
            Application = application;
            Logger = logger;
            Message = message;
            VkClient = vkClient;
            Session = session;
            ServiceTypes = serviceTypes;
        }
        
        protected async Task SendMethodUnavailableFromChat()
        {
            await Send("Команда доступна только в личных сообщениях");
        }

        protected async Task SendResultCode(Result result)
        {
            await Send(Description.Get(result.Code) ?? result.Code.ToString());
        }

        protected async Task Send(string message)
        {
            await VkClient.Messages.Send(peerId: Message.PeerId, message: message, randomId: 0);
        }
    }
}
