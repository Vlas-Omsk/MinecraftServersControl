﻿using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Core.Interface.Services;
using MinecraftServersControl.Core.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;

namespace MinecraftServersControl.Core
{
    public sealed class Application : IApplication
    {
        public IUserService UserService { get; }
        public IServerService ServerService { get; }
        public IComputerService ComputerService { get; }

        public Application(DatabaseContextFactoryBase databaseContextFactory, Logger logger, IRemoteServer remoteServer)
        {
            UserService = new UserService(this, databaseContextFactory, logger, remoteServer);
            ServerService = new ServerService(this, databaseContextFactory, logger, remoteServer);
            ComputerService = new ComputerService(this, databaseContextFactory, logger, remoteServer);
        }
    }
}
