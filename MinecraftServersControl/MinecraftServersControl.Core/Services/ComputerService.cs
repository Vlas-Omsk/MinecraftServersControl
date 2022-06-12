using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Core.Interface;
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
            remoteServer.ComputerStarted += OnRemoteServerComputerStarted;
            remoteServer.ComputerStopped += OnRemoteServerComputerStopped;
        }

        private void OnRemoteServerComputerStarted(object sender, Remote.Server.ComputerStateChangedEventArgs e)
        {
            ComputerStarted?.Invoke(this, new Result<ComputerStateDTO>(new ComputerStateDTO(e.ComputerId), ResultCode.ComputerStarted));
        }

        private void OnRemoteServerComputerStopped(object sender, Remote.Server.ComputerStateChangedEventArgs e)
        {
            ComputerStopped?.Invoke(this, new Result<ComputerStateDTO>(new ComputerStateDTO(e.ComputerId), ResultCode.ComputerStopped));
        }

        public async Task<Result> VerifyComputer(Guid computerKey)
        {
            using (var databaseContext = DatabaseContextFactory.CreateDbContext())
                if (await databaseContext.Computers.FindAsync(computerKey.ToByteArray()) != null)
                    return ResultCode.Success;

            return ResultCode.ComputerNotFound;
        }

        public event ResultEventHandler<ComputerStateDTO> ComputerStarted;
        public event ResultEventHandler<ComputerStateDTO> ComputerStopped;
    }
}
