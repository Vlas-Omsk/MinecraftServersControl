using MinecraftServersControl.Common;
using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Schema;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.Remote.Client
{
    public sealed class RemoteWebSocketClient : RemoteWebSocketClientBase
    {
        static RemoteWebSocketClient()
        {
            JsonTypeConversions.Register();
        }

        public RemoteWebSocketClient(string url, TimeSpan reconnectDelay, Logging.Logger logger, Application application) : 
            base(url, reconnectDelay, logger, application)
        {
            application.ServerService.ServerOutput += OnServerOutput;
            application.ServerService.ServerStarted += OnServerStarted;
            application.ServerService.ServerStopped += OnServerStopped;
        }

        protected override async void OnOpenOverride(object sender, EventArgs e)
        {
            base.OnOpenOverride(sender, e);

            var result = await Application.ServerService.Verify();
            SendResponse(-1, result);
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.GetInfo)]
        public async Task GetInfoAsync(RemoteWebSocketRequest request)
        {
            var result = await Application.ServerService.GetInfo();
            SendResponse(request.Id, result);
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.GetOutput)]
        public async Task GetOutputAsync(RemoteWebSocketRequest<Guid> request)
        {
            var result = await Application.ServerService.GetOutput(request.Data);
            SendResponse(request.Id, result);
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.Input)]
        public async Task GetOutputAsync(RemoteWebSocketRequest<ServerInputDTO> request)
        {
            var result = await Application.ServerService.Input(request.Data);
            SendResponse(request.Id, result);
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.Start)]
        public async Task StartAsync(RemoteWebSocketRequest<Guid> request)
        {
            var result = await Application.ServerService.Start(request.Data);
            SendResponse(request.Id, result);
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.Terminate)]
        public async Task TerminateAsync(RemoteWebSocketRequest<Guid> request)
        {
            var result = await Application.ServerService.Terminate(request.Data);
            SendResponse(request.Id, result);
        }

        private async void OnServerOutput(object sender, DTO.RemoteResult<DTO.ServerOutputDTO> e)
        {
            await Task.Run(() => SendResponse(-1, e));
        }

        private async void OnServerStarted(object sender, DTO.RemoteResult<Guid> e)
        {
            await Task.Run(() => SendResponse(-1, e));
        }

        private async void OnServerStopped(object sender, DTO.RemoteResult<Guid> e)
        {
            await Task.Run(() => SendResponse(-1, e));
        }
    }
}
