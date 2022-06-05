using MinecraftServersControl.API.Services;
using MinecraftServersControl.Core;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;

namespace MinecraftServersControl.API
{
    public sealed class ApiContextFactory
    {
        private Application _application;
        private ILogger _logger;

        public ApiContextFactory(DatabaseContextFactoryBase databaseContextFactory, ILogger logger)
        {
            _application = new Application(databaseContextFactory, logger);
            _logger = logger;
        }

        public T CreateApiService<T>(IClient client) where T : RealtimeApiService, new()
        {
            var service = new T();
            service.Init(client, _logger, _application);
            return service;
        }

        public T CreateApiService<T>() where T : ApiService, new()
        {
            var service = new T();
            service.Init(_logger, _application);
            return service;
        }
    }
}
