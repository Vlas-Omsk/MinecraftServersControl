using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Services;
using System;

namespace MinecraftServersControl.Remote
{
    public delegate void ResultEventHandler<T>(object sender, Result<T> e);

    public sealed class Application
    {
        public ServersService ServersService { get; }

        public Application(Config config, Logger logger)
        {
            ServersService = new ServersService(config, logger);
        }
    }
}
