using MinecraftServersControl.Common;
using MinecraftServersControl.Remote.Schema;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace MinecraftServersControl.Remote.Client
{
    public sealed class NetworkWebSocketClient : NetworkWebSocketClientBase
    {
        static NetworkWebSocketClient()
        {
            JsonTypeConversions.Register();
        }

        public NetworkWebSocketClient(string url, TimeSpan reconnectDelay, Logging.Logger logger, Application application) : 
            base(url, reconnectDelay, logger, application)
        {
        }

        protected override async void OnOpenOverride(object sender, EventArgs e)
        {
            base.OnOpenOverride(sender, e);

            var result = await Application.ServersService.Auth();
            SendResponse(-1, result);
        }

        protected override void OnCloseOverride(object sender, CloseEventArgs e)
        {
            base.OnCloseOverride(sender, e);
        }

        [WebSocketRequest(WebSocketRequestCode.GetInfo)]
        public async Task GetInfoAsync(WebSocketRequest request)
        {
            var result = await Application.ServersService.GetInfo();
            SendResponse(request.Id, result);
        }

        [WebSocketRequest(WebSocketRequestCode.GetOutput)]
        public async Task GetOutputAsync(WebSocketRequest<Guid> request)
        {
            var result = await Application.ServersService.GetOutput(request.Data);
            SendResponse(request.Id, result);
        }

        [WebSocketRequest(WebSocketRequestCode.Start)]
        public async Task StartAsync(WebSocketRequest<Guid> request)
        {
            var result = await Application.ServersService.Start(request.Data);
            SendResponse(request.Id, result);
        }

        [WebSocketRequest(WebSocketRequestCode.Terminate)]
        public async Task TerminateAsync(WebSocketRequest<Guid> request)
        {
            var result = await Application.ServersService.Terminate(request.Data);
            SendResponse(request.Id, result);
        }
    }
}
