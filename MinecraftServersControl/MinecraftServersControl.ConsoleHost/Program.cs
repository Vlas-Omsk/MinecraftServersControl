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
            var application = new Application(databaseContextFactory, logger);
            var networkServer = new NetworkWebsocketServer("ws://0.0.0.0:8889", logger, application);
            application.NetworkServer = networkServer;
            var server = new ApiServer(application, logger, "http://0.0.0.0:8888");

            networkServer.Start();
            server.Start();

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
