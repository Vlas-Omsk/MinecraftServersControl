using MinecraftServersControl.API;
using MinecraftServersControl.Core;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.Server;
using System;

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

            remoteServer.Start(application);
            server.Start();

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
