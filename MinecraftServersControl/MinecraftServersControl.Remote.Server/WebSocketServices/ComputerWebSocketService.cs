using MinecraftServersControl.Core;
using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Schema;
using MinecraftServersControl.Remote.Server.Schema;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebSocketSharp;

namespace MinecraftServersControl.Remote.Server.WebSocketServices
{
    public sealed class ComputerWebSocketService : WebSocketService, IRemoteComputer
    {
        public Guid ComputerKey { get; private set; }

        internal RemoteWebsocketServer RemoteServer { get; set; }

        private int _counter;

        private bool IsAuthorized => ComputerKey != default;

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);

            RemoteServer.RaiseComputerStopped(ComputerKey);
        }

        [WebSocketResponse(RemoteWebSocketResponseCode.Verify)]
        public async Task VerifyAsync(RemoteWebSocketResponse<Guid> response)
        {
            if (IsAuthorized)
                return;

            var result = await Application.ComputerService.VerifyComputer(response.Data);

            if (!result ||
                Sessions.Sessions.Any(x =>
                    x.ConnectionState == WebSocketState.Open &&
                    ((ComputerWebSocketService)x).ComputerKey == response.Data
               ))
            {
                CloseAsync();
                return;
            }

            ComputerKey = response.Data;
            RemoteServer.RaiseComputerStarted(ComputerKey);
        }

        [WebSocketResponse(RemoteWebSocketResponseCode.ServerStarted)]
        public async Task ServerStartedAsync(RemoteWebSocketResponse<Guid> response)
        {
            if (!IsAuthorized)
                return;

            await Task.Run(() => RemoteServer.RaiseServerStarted(ComputerKey, response.Data));
        }

        [WebSocketResponse(RemoteWebSocketResponseCode.ServerStopped)]
        public async Task ServerStoppedAsync(RemoteWebSocketResponse<Guid> response)
        {
            if (!IsAuthorized)
                return;

            await Task.Run(() => RemoteServer.RaiseServerStopped(ComputerKey, response.Data));
        }

        [WebSocketResponse(RemoteWebSocketResponseCode.ServerOutput)]
        public async Task ServerOutputAsync(RemoteWebSocketResponse<ServerOutputDTO> response)
        {
            if (!IsAuthorized)
                return;

            await Task.Run(() => RemoteServer.RaiseServerOutput(ComputerKey, response.Data.ServerId, response.Data.Output));
        }

        public Task<RemoteWebSocketResponse<ServerInfoDTO[]>> GetInfo()
        {
            return GetResponse<RemoteWebSocketResponse<ServerInfoDTO[]>, RemoteWebSocketRequest>(
                new RemoteWebSocketRequest(_counter++, RemoteWebSocketRequestCode.GetInfo)
            );
        }

        public Task<RemoteWebSocketResponse<string>> GetOutput(Guid serverKey)
        {
            return GetResponse<RemoteWebSocketResponse<string>, RemoteWebSocketRequest>(
                new RemoteWebSocketRequest<Guid>(_counter++, RemoteWebSocketRequestCode.GetOutput, serverKey)
            );
        }

        public Task<RemoteWebSocketResponse> Input(ServerInputDTO serverInput)
        {
            return GetResponse<RemoteWebSocketResponse, RemoteWebSocketRequest>(
               new RemoteWebSocketRequest<ServerInputDTO>(_counter++, RemoteWebSocketRequestCode.Input, serverInput)
            );
        }

        public Task<RemoteWebSocketResponse> Start(Guid serverId)
        {
            return GetResponse<RemoteWebSocketResponse, RemoteWebSocketRequest>(
                new RemoteWebSocketRequest<Guid>(_counter++, RemoteWebSocketRequestCode.Start, serverId)
            );
        }

        public Task<RemoteWebSocketResponse> Terminate(Guid serverId)
        {
            return GetResponse<RemoteWebSocketResponse, RemoteWebSocketRequest>(
                new RemoteWebSocketRequest<Guid>(_counter++, RemoteWebSocketRequestCode.Terminate, serverId)
            );
        }
    }
}
