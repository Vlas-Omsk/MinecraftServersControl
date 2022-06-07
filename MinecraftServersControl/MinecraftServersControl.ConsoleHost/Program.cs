using MinecraftServersControl.API;
using MinecraftServersControl.Core;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
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
            var mainServer = new ApiServer(application, logger, "http://0.0.0.0:8888");

            mainServer.Start();

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
