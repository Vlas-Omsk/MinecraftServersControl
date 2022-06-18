using MinecraftServersControl.Core.Abstractions;
using MinecraftServersControl.Core.Abstractions.Services;
using MinecraftServersControl.Core.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.Server.Abstractions;
using System;

namespace MinecraftServersControl.Core
{
    public sealed class Application : IApplication
    {
        public IUserService UserService { get; }
        public IVkUserService VkUserService { get; }
        public IServerService ServerService { get; }
        public IComputerService ComputerService { get; }

        public Application(DatabaseContextFactoryBase databaseContextFactory, Logger logger, IRemoteServer remoteServer)
        {
            UserService = new UserService(this, databaseContextFactory, logger, remoteServer);
            VkUserService = new VkUserService(this, databaseContextFactory, logger, remoteServer);
            ServerService = new ServerService(this, databaseContextFactory, logger, remoteServer);
            ComputerService = new ComputerService(this, databaseContextFactory, logger, remoteServer);
        }
    }
}
