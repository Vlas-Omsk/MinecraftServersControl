using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.Remote.Services
{
    public sealed class ServerService : Service
    {
        private ServerHost[] _serverHosts;
        private Config _config;

        public ServerService(Config config, Logger logger) : base(logger)
        {
            _config = config;
            _serverHosts = config.Servers.Select(x =>
            {
                var server = new ServerHost(x);
                server.DataReceived += (sender, e) => RaiseServerOutput(server.ServerInfo.Id, e.Data);
                server.Started += (sender, e) => RaiseServerStarted(server.ServerInfo.Id);
                server.Stopped += (sender, e) => RaiseServerStopped(server.ServerInfo.Id);
                return server;
            }).ToArray();
        }

        public Task<RemoteResult<Guid>> Verify()
        {
            return Task.Run(() =>
                new RemoteResult<Guid>(_config.Id, RemoteResultCode.Verify)
            );
        }

        public Task<RemoteResult<IEnumerable<ServerInfoDTO>>> GetInfo()
        {
            return Task.Run(() => 
                new RemoteResult<IEnumerable<ServerInfoDTO>>(
                    _serverHosts.Select(x => 
                        new ServerInfoDTO(x.ServerInfo.Id, x.Running)
                    ).ToArray()
                )
            );
        }

        public Task<RemoteResult<string>> GetOutput(Guid serverId)
        {
            return Task.Run<RemoteResult<string>>(() =>
            {
                var server = _serverHosts.FirstOrDefault(x => x.ServerInfo.Id == serverId);

                if (server == null)
                    return RemoteResultCode.ServerNotFound;

                return server.Buffer;
            });
        }

        public Task<RemoteResult> Input(ServerInputDTO serverInput)
        {
            return Task.Run<RemoteResult>(() =>
            {
                var server = _serverHosts.FirstOrDefault(x => x.ServerInfo.Id == serverInput.ServerId);

                if (server == null)
                    return RemoteResultCode.ServerNotFound;

                server.Input(serverInput.Message);
                return RemoteResultCode.Success;
            });
        }

        public Task<RemoteResult> Start(Guid serverId)
        {
            return Task.Run<RemoteResult>(() =>
            {
                var server = _serverHosts.FirstOrDefault(x => x.ServerInfo.Id == serverId);

                if (server == null)
                    return RemoteResultCode.ServerNotFound;

                if (server.Running)
                    return RemoteResultCode.ServerStarted;

                try
                {
                    server.Start();
                    return RemoteResultCode.Success;
                }
                catch (Exception ex)
                {
                    return new RemoteResult(RemoteResultCode.CantStartServer, ex.Message);
                }
            });
        }

        public Task<RemoteResult> Terminate(Guid serverId)
        {
            return Task.Run<RemoteResult>(() =>
            {
                var server = _serverHosts.FirstOrDefault(x => x.ServerInfo.Id == serverId);

                if (server == null)
                    return RemoteResultCode.ServerNotFound;

                if (!server.Running)
                    return RemoteResultCode.ServerStopped;

                server.Stop();
                return RemoteResultCode.Success;
            });
        }

        private void RaiseServerOutput(Guid serverKey, string data)
        {
            ServerOutput?.Invoke(this, new RemoteResult<ServerOutputDTO>(new ServerOutputDTO(serverKey, data), RemoteResultCode.ServerOutput));
        }

        private void RaiseServerStarted(Guid serverKey)
        {
            ServerStarted?.Invoke(this, new RemoteResult<Guid>(serverKey, RemoteResultCode.ServerStarted));
        }

        private void RaiseServerStopped(Guid serverKey)
        {
            ServerStopped?.Invoke(this, new RemoteResult<Guid>(serverKey, RemoteResultCode.ServerStopped));
        }

        public event ResultEventHandler<ServerOutputDTO> ServerOutput;
        public event ResultEventHandler<Guid> ServerStarted;
        public event ResultEventHandler<Guid> ServerStopped;
    }
}
