using MinecraftServersControl.API;
using MinecraftServersControl.API.Vk;
using MinecraftServersControl.Core;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.Server;
using System;
using VkApi;

namespace MinecraftServersControl.ConsoleHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var databaseContextFactory = new DatabaseContextFactory();
            var remoteServer = new RemoteWebsocketServer("ws://0.0.0.0:8889", logger);
            var application = new Application(databaseContextFactory, logger, remoteServer);
            var server = new ApiServer(application, logger, "http://0.0.0.0:8888");
            var vkClient = new VkClient("b4c3172aa829a9d7b2ea0cc83710637a4bdc34207bc13736b802ef0db6c438becf80c7cf6970942a2b15c");
            var vkServer = new VkServer(vkClient, 213893484, application, logger);

            remoteServer.Start(application);
            server.Start();
            vkServer.Start();

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
