using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Services;
using System;

namespace MinecraftServersControl.Remote
{
    public delegate void ResultEventHandler<T>(object sender, RemoteResult<T> e);

    public sealed class Application
    {
        public ServerService ServerService { get; }

        public Application(Config config, Logger logger)
        {
            ServerService = new ServerService(config, logger);
        }
    }
}
