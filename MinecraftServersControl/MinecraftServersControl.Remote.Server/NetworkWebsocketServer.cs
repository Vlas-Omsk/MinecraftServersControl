using MinecraftServersControl.Common;
using MinecraftServersControl.Core;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Logging.Adapters;
using MinecraftServersControl.Remote.Server.WebSocketServices;
using System;
using WebSocketSharp.Server;

namespace MinecraftServersControl.Remote.Server
{
    public sealed class NetworkWebsocketServer : INetworkServer
    {
        private readonly WebSocketServer _server;

        static NetworkWebsocketServer()
        {
            JsonTypeConversions.Register();
        }

        public NetworkWebsocketServer(string url, Logger logger, Application application)
        {
            _server = new WebSocketServer(url);
            _server.Log.Output = new WebSocketLoggerAdapter(logger).Output;
            _server.AddWebSocketService<ComputerWebSocketService>("/", (x) =>
            {
                x.Application = application;
                x.Logger = logger;
            });
        }

        public void Start()
        {
            _server.Start();
        }

        public INetworkComputer GetComputer(Guid computerKey)
        {
            foreach (ComputerWebSocketService session in _server.WebSocketServices["/"].Sessions.Sessions)
            {
                if (session.ConnectionState != WebSocketSharp.WebSocketState.Open ||
                    session.ComputerKey != computerKey)
                    continue;

                return session;
            }

            return null;
        }
    }
}
