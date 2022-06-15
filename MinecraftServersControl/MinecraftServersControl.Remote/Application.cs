using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.Services;
using System;

namespace MinecraftServersControl.Remote
{
    public sealed class Application
    {
        public ServerService ServerService { get; }

        public Application(Config config, Logger logger)
        {
            ServerService = new ServerService(config, logger);
        }
    }
}
