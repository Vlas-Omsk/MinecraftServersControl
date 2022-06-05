using MinecraftServersControl.API.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using PinkJson2;
using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MinecraftServersControl.API.WebSocket
{
    public sealed class MainServer
    {
        private readonly HttpServer _server;
        private readonly ApiContextFactory _apiContextFactory;
        private readonly ILogger _logger;

        static MainServer()
        {
            TypeConverter.Register(typeof(Enum), new TypeConversion((object obj, Type targetType, ref bool handled) =>
            {
                if (obj is int value)
                {
                    handled = true;
                    return Enum.ToObject(targetType, value);
                }

                return null;
            }, (object obj, Type targetType, ref bool handled) =>
            {
                handled = true;
                return Convert.ChangeType(obj, typeof(int));
            }));
        }

        public MainServer(string url)
        {
            _logger = new ConsoleLogger();
            _apiContextFactory = new ApiContextFactory(new DatabaseContextFactory(), _logger);

            _server = new MainHttpServer(_apiContextFactory, _logger, url);
            _server.Log.Output = OnServerLogOutput;
            _server.AddWebSocketService<GatewayWebSocketService>("/gateway", x =>
            {
                x.Logger = _logger;
                x.ApiService = _apiContextFactory.CreateApiService<GatewayApiService>(x);
            });
        }

        private void OnServerLogOutput(LogData data, string str)
        {
            str = data.Message + " " + str;

            switch (data.Level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Info:
                    _logger.Info(str);
                    break;
                case LogLevel.Warn:
                    _logger.Warn(str);
                    break;
                case LogLevel.Error:
                case LogLevel.Fatal:
                    _logger.Error(str);
                    break;
            }
        }

        public void Start()
        {
            _server.Start();

            _logger.Info("Server started");
        }
    }
}
