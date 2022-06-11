using Microsoft.EntityFrameworkCore;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Services
{
    public sealed class ServerService : Service
    {
        internal ServerService(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger) :
            base(application, databaseContextFactory, logger)
        {
        }

        public async Task<Result<IEnumerable<ComputerDTO>>> GetServers()
        {
            using var databaseContext = DatabaseContextFactory.CreateDbContext();
            var computers = await databaseContext.Computers.ToArrayAsync();
            var computersDto = new List<ComputerDTO>();

            foreach (var computer in computers)
            {
                var id = new Guid(computer.Id);
                var session = Application.NetworkServer.GetComputer(id);
                var serversInfo = session == null ? null : (await session.GetInfo(id)).Data;
                var serversDto = new List<ServerDTO>();

                foreach (var server in databaseContext.Servers.Where(x => x.Computer == computer))
                {
                    var serverId = new Guid(server.Id);
                    var serverInfo = serversInfo?.FirstOrDefault(x => x.Id == serverId);

                    serversDto.Add(new ServerDTO(serverId, server.Name, serverInfo?.Running ?? false));
                }

                computersDto.Add(new ComputerDTO(id, computer.Name, session != null, serversDto.ToArray()));
            }

            return new Result<IEnumerable<ComputerDTO>>(computersDto.ToArray());
        }

        public Task RaiseServerStarted(Guid computerKey, Guid serverKey)
        {
            return Task.Run(() =>
                ServerStarted?.Invoke(this, new Result<ServerStateDTO>(new ServerStateDTO(computerKey, serverKey), ResultCode.ServerStarted))
            );
        }

        public Task RaiseServerStopped(Guid computerKey, Guid serverKey)
        {
            return Task.Run(() =>
                ServerStopped?.Invoke(this, new Result<ServerStateDTO>(new ServerStateDTO(computerKey, serverKey), ResultCode.ServerStopped))
            );
        }

        public event ResultEventHandler<ServerStateDTO> ServerStarted;
        public event ResultEventHandler<ServerStateDTO> ServerStopped;
    }
}
