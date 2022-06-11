using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.Client;
using System;

namespace MinecraftServersControl.Remote.ConsoleHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = Config.Load("config.json");
            var logger = new ConsoleLogger();
            var application = new Application(config, logger);
            var client = new NetworkWebSocketClient(config.Url, TimeSpan.FromSeconds(config.ReconnectDelaySeconds), logger, application);

            client.Connect();

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}
