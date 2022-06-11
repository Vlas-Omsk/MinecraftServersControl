using MinecraftServersControl.API.HttpServices;
using MinecraftServersControl.API.WebSocketServices;
using MinecraftServersControl.Common;
using MinecraftServersControl.Core;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Logging.Adapters;
using System;

namespace MinecraftServersControl.API
{
    public sealed class ApiServer
    {
        private readonly ApiHttpServer _server;
        private readonly Logger _logger;

        static ApiServer()
        {
            JsonTypeConversions.Register();
        }

        public ApiServer(Application application, Logger logger, string url)
        {
            _logger = logger;

            _server = new ApiHttpServer(application, _logger, url);
            _server.Log.Output = new WebSocketLoggerAdapter(logger).Output;
            _server.AddWebSocketService<GatewayWebSocketService>("/gateway", x =>
            {
                x.Logger = _logger;
                x.Application = application;
            });
            _server.AddHttpService<UserHttpService>("/user");
        }

        public void Start()
        {
            _server.Start();

            _logger.Info("Server started");
        }
    }
}
