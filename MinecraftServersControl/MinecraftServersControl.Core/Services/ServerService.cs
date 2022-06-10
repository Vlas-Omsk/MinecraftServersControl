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
        internal ServerService(Application application, DatabaseContextFactoryBase databaseContextFactory, ILogger logger) :
            base(application, databaseContextFactory, logger)
        {
        }

        public async Task<Result<IEnumerable<ComputerDTO>>> GetServers(Guid sessionId)
        {
            var result = await Application.UserService.VerifySession(sessionId);

            if (result.HasErrors())
                return result.ToResult<IEnumerable<ComputerDTO>>();

            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
                return new Result<IEnumerable<ComputerDTO>>(
                    await databaseContext.Computers
                        .Select(x => new ComputerDTO(
                            x.Name,
                            databaseContext.Servers
                                .Where(c => c.ComputerId == x.Id)
                                .Select(c => new ServerDTO(c.Name))
                                .ToArray()
                        ))
                        .ToArrayAsync()
                    );
        }
    }
}
