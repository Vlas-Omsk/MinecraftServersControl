using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;

namespace MinecraftServersControl.Core.Services
{
    public abstract class Service
    {
        protected Logger Logger { get; }
        protected Application Application { get; }
        protected DatabaseContextFactoryBase DatabaseContextFactory { get; }
        protected IRemoteServer RemoteServer { get; }

        protected Service(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger, IRemoteServer remoteServer)
        {
            Application = application;
            Logger = logger;
            DatabaseContextFactory = databaseContextFactory;
            RemoteServer = remoteServer;
        }
    }
}
