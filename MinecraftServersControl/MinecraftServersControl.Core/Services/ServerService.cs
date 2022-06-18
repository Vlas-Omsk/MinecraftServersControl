using Microsoft.EntityFrameworkCore;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Abstractions;
using MinecraftServersControl.Core.Abstractions.Services;
using MinecraftServersControl.Core.Models;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinecraftServersControl.Remote.Server.Abstractions;

namespace MinecraftServersControl.Core.Services
{
    public sealed class ServerService : Service, IServerService
    {
        internal ServerService(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger, IRemoteServer remoteServer) :
            base(application, databaseContextFactory, logger, remoteServer)
        {
            remoteServer.ServerStarted += async (sender, e) =>
            {
                var computerServerAliases = await GetComputerServerAliases(e.ComputerId, e.ServerId);
                ServerStarted?.Invoke(this, new TargetServerDTO(computerServerAliases.ComputerAlias, computerServerAliases.ServerAlias));
            };
            remoteServer.ServerStopped += async (sender, e) =>
            {
                var computerServerAliases = await GetComputerServerAliases(e.ComputerId, e.ServerId);
                ServerStopped?.Invoke(this, new TargetServerDTO(computerServerAliases.ComputerAlias, computerServerAliases.ServerAlias));
            };
            remoteServer.ServerOutput += async (sender, e) =>
            {
                var computerServerAliases = await GetComputerServerAliases(e.ComputerId, e.ServerId);
                ServerOutput?.Invoke(this, new DTO.ServerOutputDTO(computerServerAliases.ComputerAlias, computerServerAliases.ServerAlias, e.Output));
            };
        }

        public async Task<ComputerDTO[]> GetServers()
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

            return computersDto.ToArray();
        }

        public async Task<string> GetOutput(TargetServerDTO targetServer)
        {
            var computerServerId = await GetComputerServerId(targetServer.ComputerAlias, targetServer.ServerAlias);
            var response = await computerServerId.Computer.GetOutput(computerServerId.ServerId);

            response.ThrowOnError();

            return response.Data;
        }

        public async Task Input(ServerInputDTO serverInput)
        {
            var computerServerId = await GetComputerServerId(serverInput.ComputerAlias, serverInput.ServerAlias);
            var response = await computerServerId.Computer.Input(new Remote.Core.DTO.ServerInputDTO(computerServerId.ServerId, serverInput.Message));

            response.ThrowOnError();
        }

        public async Task Start(TargetServerDTO targetServer)
        {
            var computerServerId = await GetComputerServerId(targetServer.ComputerAlias, targetServer.ServerAlias);
            var response = await computerServerId.Computer.Start(computerServerId.ServerId);

            response.ThrowOnError();
        }

        public async Task Terminate(TargetServerDTO targetServer)
        {
            var computerServerId = await GetComputerServerId(targetServer.ComputerAlias, targetServer.ServerAlias);
            var response = await computerServerId.Computer.Terminate(computerServerId.ServerId);

            response.ThrowOnError();
        }

        private async Task<ComputerServerIdPair> GetComputerServerId(string computerAlias, string serverAlias)
        {
            using var databaseContext = DatabaseContextFactory.CreateDbContext();
            var computer = await databaseContext.Computers.FirstOrDefaultAsync(x => x.Alias == computerAlias);

            if (computer == null)
                throw new CoreException(ErrorCode.ComputerNotFound);

            var server = await databaseContext.Servers.FirstOrDefaultAsync(x => x.Alias == serverAlias);
            if (server == null)
                throw new CoreException(ErrorCode.ServerNotFound);

            var remoteComputer = RemoteServer.GetComputer(new Guid(computer.Id));

            if (remoteComputer == null)
                throw new CoreException(ErrorCode.ComputerAlredyStopped);

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

        public event EventHandler<TargetServerDTO> ServerStarted;
        public event EventHandler<TargetServerDTO> ServerStopped;
        public event EventHandler<ServerOutputDTO> ServerOutput;
    }
}
