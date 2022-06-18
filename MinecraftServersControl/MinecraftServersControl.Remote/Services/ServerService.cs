using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.Core.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.Remote.Core.Services
{
    public sealed class ServerService : Service
    {
        private ServerHost[] _serverHosts;
        private Config _config;

        internal ServerService(Config config, Logger logger) : base(logger)
        {
            _config = config;
            _serverHosts = config.Servers.Select(x =>
            {
                var server = new ServerHost(x);
                server.DataReceived += (sender, e) => ServerOutput?.Invoke(this, new ServerOutputDTO(server.ServerInfo.Id, e.Data));
                server.Started += (sender, e) => ServerStarted?.Invoke(this, server.ServerInfo.Id);
                server.Stopped += (sender, e) => ServerStopped?.Invoke(this, server.ServerInfo.Id);
                return server;
            }).ToArray();
        }

        public Task<Guid> Verify()
        {
            return Task.Run(() =>
                _config.Id
            );
        }

        public Task<ServerInfoDTO[]> GetInfo()
        {
            return Task.Run(() => 
                _serverHosts.Select(x => 
                    new ServerInfoDTO(x.ServerInfo.Id, x.Running)
                ).ToArray()
            );
        }

        public Task<string> GetOutput(Guid serverId)
        {
            return Task.Run<string>(() =>
            {
                var server = _serverHosts.FirstOrDefault(x => x.ServerInfo.Id == serverId);

                if (server == null)
                    throw new RemoteCoreException(RemoteErrorCode.ServerNotFound);

                return server.Buffer;
            });
        }

        public Task Input(ServerInputDTO serverInput)
        {
            return Task.Run(() =>
            {
                var server = _serverHosts.FirstOrDefault(x => x.ServerInfo.Id == serverInput.ServerId);

                if (server == null)
                    throw new RemoteCoreException(RemoteErrorCode.ServerNotFound);

                server.Input(serverInput.Message);
            });
        }

        public Task Start(Guid serverId)
        {
            return Task.Run(() =>
            {
                var server = _serverHosts.FirstOrDefault(x => x.ServerInfo.Id == serverId);

                if (server == null)
                    throw new RemoteCoreException(RemoteErrorCode.ServerNotFound);

                if (server.Running)
                    throw new RemoteCoreException(RemoteErrorCode.ServerAlredyStarted);

                try
                {
                    server.Start();
                }
                catch (Exception)
                {
                    throw new RemoteCoreException(RemoteErrorCode.CantStartServer);
                }
            });
        }

        public Task Terminate(Guid serverId)
        {
            return Task.Run(() =>
            {
                var server = _serverHosts.FirstOrDefault(x => x.ServerInfo.Id == serverId);

                if (server == null)
                    throw new RemoteCoreException(RemoteErrorCode.ServerNotFound);

                if (!server.Running)
                    throw new RemoteCoreException(RemoteErrorCode.ServerAlredyStopped);

                server.Stop();
            });
        }

        public event EventHandler<ServerOutputDTO> ServerOutput;
        public event EventHandler<Guid> ServerStarted;
        public event EventHandler<Guid> ServerStopped;
    }
}
