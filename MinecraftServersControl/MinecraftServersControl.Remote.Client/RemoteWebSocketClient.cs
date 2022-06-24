using MinecraftServersControl.Common;
using MinecraftServersControl.Remote.Core;
using MinecraftServersControl.Remote.Core.DTO;
using MinecraftServersControl.Remote.Server.Schema;
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
            SendResponse(RemoteWebSocketResponse.CreateBroadcast(RemoteWebSocketResponseCode.Verify, result));
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.GetInfo)]
        public async Task GetInfoAsync(RemoteWebSocketRequest request)
        {
            var result = await Application.ServerService.GetInfo();
            SendResponse(RemoteWebSocketResponse.CreateSuccess(request.Id, result));
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.GetOutput)]
        public async Task GetOutputAsync(RemoteWebSocketRequest<Guid> request)
        {
            var result = await Application.ServerService.GetOutput(request.Data);
            SendResponse(RemoteWebSocketResponse.CreateSuccess(request.Id, result));
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.Input)]
        public async Task GetOutputAsync(RemoteWebSocketRequest<ServerInputDTO> request)
        {
            await Application.ServerService.Input(request.Data);
            SendResponse(RemoteWebSocketResponse.CreateSuccess(request.Id));
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.Start)]
        public async Task StartAsync(RemoteWebSocketRequest<Guid> request)
        {
            await Application.ServerService.Start(request.Data);
            SendResponse(RemoteWebSocketResponse.CreateSuccess(request.Id));
        }

        [WebSocketRequest(RemoteWebSocketRequestCode.Terminate)]
        public async Task TerminateAsync(RemoteWebSocketRequest<Guid> request)
        {
            await Application.ServerService.Terminate(request.Data);
            SendResponse(RemoteWebSocketResponse.CreateSuccess(request.Id));
        }

        private async void OnServerOutput(object sender, ServerOutputDTO e)
        {
            await Task.Run(() => SendResponse(RemoteWebSocketResponse.CreateBroadcast(RemoteWebSocketResponseCode.ServerOutput, e)));
        }

        private async void OnServerStarted(object sender, Guid e)
        {
            await Task.Run(() => SendResponse(RemoteWebSocketResponse.CreateBroadcast(RemoteWebSocketResponseCode.ServerStarted, e)));
        }

        private async void OnServerStopped(object sender, Guid e)
        {
            await Task.Run(() => SendResponse(RemoteWebSocketResponse.CreateBroadcast(RemoteWebSocketResponseCode.ServerStopped, e)));
        }
    }
}
