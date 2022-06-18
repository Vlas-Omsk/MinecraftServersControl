using MinecraftServersControl.Common;
using MinecraftServersControl.Core.Abstractions;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Logging.Adapters;
using MinecraftServersControl.Remote.Server.Abstractions;
using MinecraftServersControl.Remote.Server.WebSocketServices;
using System;
using WebSocketSharp.Server;

namespace MinecraftServersControl.Remote.Server
{
    public sealed class RemoteWebsocketServer : IRemoteServer
    {
        private readonly WebSocketServer _server;
        private IApplication _application;
        private Logger _logger;

        static RemoteWebsocketServer()
        {
            JsonTypeConversions.Register();
        }

        public RemoteWebsocketServer(string url, Logger logger)
        {
            _logger = logger;
            _server = new WebSocketServer(url);
            _server.Log.Output = new WebSocketLoggerAdapter(logger).Output;
            _server.AddWebSocketService<ComputerWebSocketService>("/", (x) =>
            {
                x.Application = _application;
                x.Logger = logger;
                x.RemoteServer = this;
            });
        }

        public void Start(IApplication application)
        {
            _application = application;
            _server.Start();

            _logger.Info("Server started");
        }

        public IRemoteComputer GetComputer(Guid computerId)
        {
            foreach (ComputerWebSocketService session in _server.WebSocketServices["/"].Sessions.Sessions)
            {
                if (session.ConnectionState != WebSocketSharp.WebSocketState.Open ||
                    session.ComputerKey != computerId)
                    continue;

                return session;
            }

            return null;
        }

        internal void RaiseComputerStarted(Guid computerId)
        {
            ComputerStarted?.Invoke(this, new ComputerStateChangedEventArgs(computerId));
        }

        internal void RaiseComputerStopped(Guid computerId)
        {
            ComputerStopped?.Invoke(this, new ComputerStateChangedEventArgs(computerId));
        }

        internal void RaiseServerStarted(Guid computerId, Guid serverId)
        {
            ServerStarted?.Invoke(this, new ServerStateChangedEventArgs(computerId, serverId));
        }

        internal void RaiseServerStopped(Guid computerId, Guid serverId)
        {
            ServerStopped?.Invoke(this, new ServerStateChangedEventArgs(computerId, serverId));
        }

        internal void RaiseServerOutput(Guid computerId, Guid serverId, string message)
        {
            ServerOutput?.Invoke(this, new ServerOutputEventArgs(computerId, serverId, message));
        }

        public event EventHandler<ComputerStateChangedEventArgs> ComputerStarted;
        public event EventHandler<ComputerStateChangedEventArgs> ComputerStopped;
        public event EventHandler<ServerStateChangedEventArgs> ServerStarted;
        public event EventHandler<ServerStateChangedEventArgs> ServerStopped;
        public event EventHandler<ServerOutputEventArgs> ServerOutput;
    }
}
