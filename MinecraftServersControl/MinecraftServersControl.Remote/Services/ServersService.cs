using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.Remote.Services
{
    public sealed class ServersService : Service
    {
        private IEnumerable<Server> _servers;
        private Config _config;

        public ServersService(Config config, Logger logger) : base(logger)
        {
            _config = config;
            _servers = config.Servers.Select(x =>
            {
                var server = new Server(x);
                server.DataReceived += (sender, e) => RaiseServerOutput(server.ServerInfo.Id, e.Data);
                server.Started += (sender, e) => RaiseServerStarted(server.ServerInfo.Id);
                server.Stopped += (sender, e) => RaiseServerStopped(server.ServerInfo.Id);
                return server;
            });
        }

        public Task<Result<Guid>> Auth()
        {
            return Task.Run(() =>
                new Result<Guid>(_config.Id, ResultCode.Verify)
            );
        }

        public Task<Result<IEnumerable<ServerDTO>>> GetInfo()
        {
            return Task.Run(() => 
                new Result<IEnumerable<ServerDTO>>(
                    _servers.Select(x => 
                        new ServerDTO(x.ServerInfo.Id, x.Running)
                    ).ToArray()
                )
            );
        }

        public Task<Result<string>> GetOutput(Guid serverKey)
        {
            return Task.Run<Result<string>>(() =>
            {
                var server = _servers.FirstOrDefault(x => x.ServerInfo.Id == serverKey);

                if (server == null)
                    return ResultCode.ServerNotFound;

                return server.Buffer;
            });
        }

        public Task<Result> Start(Guid serverKey)
        {
            return Task.Run<Result>(() =>
            {
                var server = _servers.FirstOrDefault(x => x.ServerInfo.Id == serverKey);

                if (server == null)
                    return ResultCode.ServerNotFound;

                if (server.Running)
                    return ResultCode.ServerStarted;

                try
                {
                    server.Start();
                    return ResultCode.Success;
                }
                catch (Exception ex)
                {
                    return new Result(ResultCode.CantStartServer, ex.Message);
                }
            });
        }

        public Task<Result> Terminate(Guid serverKey)
        {
            return Task.Run<Result>(() =>
            {
                var server = _servers.FirstOrDefault(x => x.ServerInfo.Id == serverKey);

                if (server == null)
                    return ResultCode.ServerNotFound;

                if (!server.Running)
                    return ResultCode.ServerStopped;

                server.Stop();
                return ResultCode.Success;
            });
        }

        private void RaiseServerOutput(Guid serverKey, string data)
        {
            ServerOutput?.Invoke(this, new ServerOutputDTO(serverKey, data));
        }

        private void RaiseServerStarted(Guid serverKey)
        {
            ServerStarted?.Invoke(this, serverKey);
        }

        private void RaiseServerStopped(Guid serverKey)
        {
            ServerStopped?.Invoke(this, serverKey);
        }

        public event ResultEventHandler<ServerOutputDTO> ServerOutput;
        public event ResultEventHandler<Guid> ServerStarted;
        public event ResultEventHandler<Guid> ServerStopped;
    }
}
