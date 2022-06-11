using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;

namespace MinecraftServersControl.Core
{
    public delegate void ResultEventHandler<T>(object sender, Result<T> e);

    public sealed class Application
    {
        public UserService UserService { get; }
        public ServerService ServerService { get; }
        public ComputerService ComputerService { get; }
        public INetworkServer NetworkServer { get; set; }

        public Application(DatabaseContextFactoryBase databaseContextFactory, Logger logger)
        {
            UserService = new UserService(this, databaseContextFactory, logger);
            ServerService = new ServerService(this, databaseContextFactory, logger);
            ComputerService = new ComputerService(this, databaseContextFactory, logger);
        }
    }
}
