using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Core.Services
{
    public sealed class ComputerService : Service, IComputerService
    {
        internal ComputerService(Application application, DatabaseContextFactoryBase databaseContextFactory, Logger logger, IRemoteServer remoteServer) :
            base(application, databaseContextFactory, logger, remoteServer)
        {
            remoteServer.ComputerStarted += (sender, e) => ComputerStarted?.Invoke(this, new ComputerStateDTO(e.ComputerId));
            remoteServer.ComputerStopped += (sender, e) => ComputerStopped?.Invoke(this, new ComputerStateDTO(e.ComputerId));
        }

        public async Task<bool> VerifyComputer(Guid computerKey)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
                if (await databaseContext.Computers.FindAsync(computerKey.ToByteArray()) != null)
                    return true;

            return false;
        }

        public event EventHandler<ComputerStateDTO> ComputerStarted;
        public event EventHandler<ComputerStateDTO> ComputerStopped;
    }
}
