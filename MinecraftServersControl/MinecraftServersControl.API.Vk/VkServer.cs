using MinecraftServersControl.API.Vk.VkServices;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Logging;
using System;
using VkApi;
using VkApi.Models;

namespace MinecraftServersControl.API.Vk
{
    public sealed class VkServer
    {
        private readonly IApplication _application;
        private readonly Logger _logger;
        private readonly VkClient _vkClient;
        private readonly GroupsLongPollServer _longPollServer;
        private readonly Type[] _serviceTypes = new Type[]
        {
            typeof(UserVkService),
            typeof(ComputerVkService),
            typeof(ServerVkService),
            typeof(CommonVkService)
        };
        private readonly VkSessionStorage _sessionStorage = new VkSessionStorage();

        public VkServer(VkClient vkClient, int groupId, IApplication application, Logger logger)
        {
            _vkClient = vkClient;
            _application = application;
            _logger = logger;

            _longPollServer = vkClient.Groups.GetLongPollServer(groupId);
            _longPollServer.Update += OnLongPollServerUpdate;
        }

        private async void OnLongPollServerUpdate(object sender, LongPollUpdateEventArgs e)
        {
            foreach (var update in e.Updates)
            {
                if (update.Type == LongPollUpdateType.MessageNew)
                {
                    var messageNew = (LongPollMessageNew)update.Object;
                    var handler = new VkMessageHandler(_serviceTypes, messageNew.Message, _sessionStorage, _vkClient, _application, _logger);
                    await handler.ProcessMessage();
                }
            }
        }

        public void Start()
        {
            _longPollServer.Start();

            _logger.Info("Server started");
        }
    }
}
