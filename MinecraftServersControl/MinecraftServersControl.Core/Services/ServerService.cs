using Microsoft.EntityFrameworkCore;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Core.Interface.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Services
{
    public sealed class ServerService : Service, IServerService
    {
        internal ServerService(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger, IRemoteServer remoteServer) :
            base(application, databaseContextFactory, logger, remoteServer)
        {
            remoteServer.ServerStarted += OnRemoteServerServerStarted;
            remoteServer.ServerStopped += OnRemoteServerServerStopped;
            remoteServer.ServerOutput += OnRemoteServerServerOutput;
        }

        private void OnRemoteServerServerStarted(object sender, Remote.Server.ServerStateChangedEventArgs e)
        {
            ServerStarted?.Invoke(this, new Result<TargetServerDTO>(new TargetServerDTO(e.ComputerId, e.ServerId), ResultCode.ServerStarted));
        }

        private void OnRemoteServerServerStopped(object sender, Remote.Server.ServerStateChangedEventArgs e)
        {
            ServerStopped?.Invoke(this, new Result<TargetServerDTO>(new TargetServerDTO(e.ComputerId, e.ServerId), ResultCode.ServerStopped));
        }

        private void OnRemoteServerServerOutput(object sender, Remote.Server.Interface.ServerOutputEventArgs e)
        {
            ServerOutput?.Invoke(this, new Result<DTO.ServerOutputDTO>(new DTO.ServerOutputDTO(e.ComputerId, e.ServerId, e.Output), ResultCode.ServerOutput));
        }

        public async Task<Result<IEnumerable<ComputerDTO>>> GetServers()
        {
            using var databaseContext = DatabaseContextFactory.CreateDbContext();
            var computers = await databaseContext.Computers.ToArrayAsync();
            var computersDto = new List<ComputerDTO>();

            foreach (var computer in computers)
            {
                var id = new Guid(computer.Id);
                var remoteComputer = RemoteServer.GetComputer(id);
                var serversInfo = remoteComputer == null ? null : (await remoteComputer.GetInfo(id)).Data;
                var serversDto = new List<ServerDTO>();

                foreach (var server in databaseContext.Servers.Where(x => x.Computer == computer))
                {
                    var serverId = new Guid(server.Id);
                    var serverInfo = serversInfo?.FirstOrDefault(x => x.Id == serverId);

                    serversDto.Add(new ServerDTO(serverId, server.Name, serverInfo?.Running ?? false));
                }

                computersDto.Add(new ComputerDTO(id, computer.Name, remoteComputer != null, serversDto.ToArray()));
            }

            return new Result<IEnumerable<ComputerDTO>>(computersDto.ToArray());
        }

        public async Task<Result<string>> GetOutput(TargetServerDTO serverState)
        {
            var remoteComputer = RemoteServer.GetComputer(serverState.ComputerId);

            if (remoteComputer == null)
                return ResultCode.ComputerNotFound;

            var result = await remoteComputer.GetOutput(serverState.ComputerId, serverState.ServerId);

            return result.FromRemoteResult(RemoteResultCode.ServerNotFound, RemoteResultCode.Success);
        }

        public async Task<Result> Input(DTO.ServerInputDTO serverInput)
        {
            var remoteComputer = RemoteServer.GetComputer(serverInput.ComputerId);

            if (remoteComputer == null)
                return ResultCode.ComputerNotFound;

            var result = await remoteComputer.Input(serverInput.ComputerId, new Remote.DTO.ServerInputDTO(serverInput.ServerId, serverInput.Message));

            return result.FromRemoteResult(RemoteResultCode.ServerNotFound, RemoteResultCode.Success);
        }

        public async Task<Result> Start(TargetServerDTO serverState)
        {
            var remoteComputer = RemoteServer.GetComputer(serverState.ComputerId);

            if (remoteComputer == null)
                return ResultCode.ComputerNotFound;

            var result = await remoteComputer.Start(serverState.ComputerId, serverState.ServerId);

            return result.FromRemoteResult(
                RemoteResultCode.ServerNotFound,
                RemoteResultCode.ServerStarted,
                RemoteResultCode.Success,
                RemoteResultCode.CantStartServer
            );
        }

        public async Task<Result> Terminate(TargetServerDTO serverState)
        {
            var remoteComputer = RemoteServer.GetComputer(serverState.ComputerId);

            if (remoteComputer == null)
                return ResultCode.ComputerNotFound;

            var result = await remoteComputer.Terminate(serverState.ComputerId, serverState.ServerId);

            return result.FromRemoteResult(
                RemoteResultCode.ServerNotFound,
                RemoteResultCode.ServerStopped,
                RemoteResultCode.Success
            );
        }

        public event ResultEventHandler<TargetServerDTO> ServerStarted;
        public event ResultEventHandler<TargetServerDTO> ServerStopped;
        public event ResultEventHandler<DTO.ServerOutputDTO> ServerOutput;
    }
}
