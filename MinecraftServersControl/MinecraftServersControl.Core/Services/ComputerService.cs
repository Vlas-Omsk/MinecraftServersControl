using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Services
{
    public sealed class ComputerService : Service
    {
        internal ComputerService(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger) :
            base(application, databaseContextFactory, logger)
        {
        }

        public async Task<Result> VerifyComputer(Guid computerKey)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
                if (await databaseContext.Computers.FindAsync(computerKey.ToByteArray()) != null)
                    return ResultCode.Success;

            return ResultCode.ComputerNotFound;
        }
    }
}
