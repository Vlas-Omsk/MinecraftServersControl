using Microsoft.EntityFrameworkCore;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Core.Interface.Services;
using MinecraftServersControl.Core.Models;
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

        private async void OnRemoteServerServerStarted(object sender, Remote.Server.ServerStateChangedEventArgs e)
        {
            var computerServerAliases = await GetComputerServerAliases(e.ComputerId, e.ServerId);

            ServerStarted?.Invoke(this, new Result<TargetServerDTO>(new TargetServerDTO(computerServerAliases.ComputerAlias, computerServerAliases.ServerAlias), ResultCode.ServerStarted));
        }

        private async void OnRemoteServerServerStopped(object sender, Remote.Server.ServerStateChangedEventArgs e)
        {
            var computerServerAliases = await GetComputerServerAliases(e.ComputerId, e.ServerId);

            ServerStopped?.Invoke(this, new Result<TargetServerDTO>(new TargetServerDTO(computerServerAliases.ComputerAlias, computerServerAliases.ServerAlias), ResultCode.ServerStopped));
        }

        private async void OnRemoteServerServerOutput(object sender, Remote.Server.Interface.ServerOutputEventArgs e)
        {
            var computerServerAliases = await GetComputerServerAliases(e.ComputerId, e.ServerId);

            ServerOutput?.Invoke(this, new Result<DTO.ServerOutputDTO>(new DTO.ServerOutputDTO(computerServerAliases.ComputerAlias, computerServerAliases.ServerAlias, e.Output), ResultCode.ServerOutput));
        }

        public async Task<Result<IEnumerable<ComputerDTO>>> GetServers()
        {
            using var databaseContext = DatabaseContextFactory.CreateDbContext();
            var computers = await databaseContext.Computers.ToArrayAsync();
            var computersDto = new List<ComputerDTO>();

            foreach (var computer in computers)
            {
                var remoteComputer = RemoteServer.GetComputer(new Guid(computer.Id));
                var serversInfo = remoteComputer == null ? null : (await remoteComputer.GetInfo()).Data;
                var serversDto = new List<ServerDTO>();

                foreach (var server in databaseContext.Servers.Where(x => x.Computer == computer))
                {
                    var serverId = new Guid(server.Id);
                    var serverInfo = serversInfo?.FirstOrDefault(x => x.Id == serverId);

                    serversDto.Add(new ServerDTO(server.Alias, server.Name, serverInfo?.Running ?? false));
                }

                computersDto.Add(new ComputerDTO(computer.Name, computer.Alias, remoteComputer != null, serversDto.ToArray()));
            }

            return new Result<IEnumerable<ComputerDTO>>(computersDto.ToArray());
        }

        public async Task<Result<string>> GetOutput(TargetServerDTO targetServer)
        {
            var computerServerId = await GetComputerServerId(targetServer.ComputerAlias, targetServer.ServerAlias);
            if (computerServerId.HasErrors())
                return computerServerId.ToResult<string>();

            var result = await computerServerId.Data.Computer.GetOutput(computerServerId.Data.ServerId);

            return result.FromRemoteResult(RemoteResultCode.ServerNotFound, RemoteResultCode.Success);
        }

        public async Task<Result> Input(DTO.ServerInputDTO serverInput)
        {
            var computerServerId = await GetComputerServerId(serverInput.ComputerAlias, serverInput.ServerAlias);
            if (computerServerId.HasErrors())
                return computerServerId.ToResult();

            var result = await computerServerId.Data.Computer.Input(new Remote.DTO.ServerInputDTO(computerServerId.Data.ServerId, serverInput.Message));

            return result.FromRemoteResult(RemoteResultCode.ServerNotFound, RemoteResultCode.Success);
        }

        public async Task<Result> Start(TargetServerDTO targetServer)
        {
            var computerServerId = await GetComputerServerId(targetServer.ComputerAlias, targetServer.ServerAlias);
            if (computerServerId.HasErrors())
                return computerServerId.ToResult();

            var result = await computerServerId.Data.Computer.Start(computerServerId.Data.ServerId);

            return result.FromRemoteResult(
                RemoteResultCode.ServerNotFound,
                RemoteResultCode.ServerStarted,
                RemoteResultCode.Success,
                RemoteResultCode.CantStartServer
            );
        }

        public async Task<Result> Terminate(TargetServerDTO targetServer)
        {
            var computerServerId = await GetComputerServerId(targetServer.ComputerAlias, targetServer.ServerAlias);
            if (computerServerId.HasErrors())
                return computerServerId.ToResult();

            var result = await computerServerId.Data.Computer.Terminate(computerServerId.Data.ServerId);

            return result.FromRemoteResult(
                RemoteResultCode.ServerNotFound,
                RemoteResultCode.ServerStopped,
                RemoteResultCode.Success
            );
        }

        private async Task<Result<ComputerServerIdPair>> GetComputerServerId(string computerAlias, string serverAlias)
        {
            using var databaseContext = DatabaseContextFactory.CreateDbContext();
            var computer = await databaseContext.Computers.FirstOrDefaultAsync(x => x.Alias == computerAlias);

            if (computer == null)
                return ResultCode.ComputerNotFound;

            var server = await databaseContext.Servers.FirstOrDefaultAsync(x => x.Alias == serverAlias);
            if (server == null)
                return ResultCode.ServerNotFound;

            var remoteComputer = RemoteServer.GetComputer(new Guid(computer.Id));

            if (remoteComputer == null)
                return ResultCode.ComputerStopped;

            return new ComputerServerIdPair(remoteComputer, new Guid(server.Id));
        }

        private async Task<ComputerServerAliasesPair> GetComputerServerAliases(Guid computerId, Guid serverId)
        {
            using var databaseContext = DatabaseContextFactory.CreateDbContext();

            return new ComputerServerAliasesPair(
                (await databaseContext.Computers.FindAsync(computerId.ToByteArray())).Alias,
                (await databaseContext.Servers.FindAsync(serverId.ToByteArray())).Alias
            );
        }

        public event ResultEventHandler<TargetServerDTO> ServerStarted;
        public event ResultEventHandler<TargetServerDTO> ServerStopped;
        public event ResultEventHandler<DTO.ServerOutputDTO> ServerOutput;
    }
}
