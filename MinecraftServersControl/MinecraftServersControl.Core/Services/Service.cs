using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;

namespace MinecraftServersControl.Core.Services
{
    public abstract class Service
    {
        protected ILogger Logger { get; }
        protected Application Application { get; }
        protected DatabaseContextFactoryBase DatabaseContextFactory { get; }

        protected Service(Application application, DatabaseContextFactoryBase databaseContextFactory, ILogger logger)
        {
            Application = application;
            Logger = logger;
            DatabaseContextFactory = databaseContextFactory;
        }
    }
}
