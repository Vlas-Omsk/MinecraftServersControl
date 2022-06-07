using MinecraftServersControl.API.HttpServices;
using MinecraftServersControl.API.WebSocketServices;
using MinecraftServersControl.Core;
using MinecraftServersControl.Logging;
using PinkJson2;
using System;
using WebSocketSharp;

namespace MinecraftServersControl.API
{
    public sealed class ApiServer
    {
        private readonly ApiHttpServer _server;
        private readonly ILogger _logger;

        static ApiServer()
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

        public ApiServer(Application application, ILogger logger, string url)
        {
            _logger = logger;

            _server = new ApiHttpServer(application, _logger, url);
            _server.Log.Output = OnLogOutput;
            _server.AddWebSocketService<GatewayWebSocketService>("/gateway", x =>
            {
                x.Logger = _logger;
                x.Application = application;
            });
            _server.AddHttpService<UserHttpService>("/user");
        }

        private void OnLogOutput(LogData data, string str)
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
