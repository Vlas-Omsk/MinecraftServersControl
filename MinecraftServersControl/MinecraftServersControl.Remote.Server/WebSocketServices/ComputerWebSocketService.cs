using MinecraftServersControl.Core;
using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Schema;
using System;
using System.Collections.Generic;
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

        [WebSocketResponse(RemoteResultCode.Verify)]
        public async Task VerifyAsync(RemoteWebSocketResponse<RemoteResult<Guid>> response)
        {
            if (IsAuthorized)
                return;

            var result = await Application.ComputerService.VerifyComputer(response.Result.Data);

            if (result.HasErrors() ||
                Sessions.Sessions.Any(x =>
                    x.ConnectionState == WebSocketSharp.WebSocketState.Open &&
                    ((ComputerWebSocketService)x).ComputerKey == response.Result.Data
               ))
            {
                CloseAsync();
                return;
            }

            ComputerKey = response.Result.Data;
            RemoteServer.RaiseComputerStarted(ComputerKey);
        }

        [WebSocketResponse(RemoteResultCode.ServerStarted)]
        public async Task ServerStartedAsync(RemoteWebSocketResponse<RemoteResult<Guid>> response)
        {
            if (!IsAuthorized)
                return;

            await Task.Run(() => RemoteServer.RaiseServerStarted(ComputerKey, response.Result.Data));
        }

        [WebSocketResponse(RemoteResultCode.ServerStopped)]
        public async Task ServerStoppedAsync(RemoteWebSocketResponse<RemoteResult<Guid>> response)
        {
            if (!IsAuthorized)
                return;

            await Task.Run(() => RemoteServer.RaiseServerStopped(ComputerKey, response.Result.Data));
        }

        [WebSocketResponse(RemoteResultCode.ServerOutput)]
        public async Task ServerOutputAsync(RemoteWebSocketResponse<RemoteResult<ServerOutputDTO>> response)
        {
            if (!IsAuthorized)
                return;

            await Task.Run(() => RemoteServer.RaiseServerOutput(ComputerKey, response.Result.Data.ServerId, response.Result.Data.Output));
        }

        public async Task<RemoteResult<IEnumerable<ServerInfoDTO>>> GetInfo(Guid computerKey)
        {
            return (await GetResponse<RemoteWebSocketResponse<RemoteResult<IEnumerable<ServerInfoDTO>>>, RemoteWebSocketRequest>(
                new RemoteWebSocketRequest(_counter++, RemoteWebSocketRequestCode.GetInfo)
            )).Result;
        }

        public async Task<RemoteResult<string>> GetOutput(Guid computerKey, Guid serverKey)
        {
            return (await GetResponse<RemoteWebSocketResponse<RemoteResult<string>>, RemoteWebSocketRequest>(
                new RemoteWebSocketRequest<Guid>(_counter++, RemoteWebSocketRequestCode.GetOutput, serverKey)
            )).Result;
        }

        public async Task<RemoteResult> Input(Guid computerId, ServerInputDTO serverInput)
        {
            return (await GetResponse<RemoteWebSocketResponse<RemoteResult>, RemoteWebSocketRequest>(
               new RemoteWebSocketRequest<ServerInputDTO>(_counter++, RemoteWebSocketRequestCode.Input, serverInput)
           )).Result;
        }

        public async Task<RemoteResult> Start(Guid computerKey, Guid serverKey)
        {
            return (await GetResponse<RemoteWebSocketResponse<RemoteResult>, RemoteWebSocketRequest>(
                new RemoteWebSocketRequest<Guid>(_counter++, RemoteWebSocketRequestCode.Start, serverKey)
            )).Result;
        }

        public async Task<RemoteResult> Terminate(Guid computerKey, Guid serverKey)
        {
            return (await GetResponse<RemoteWebSocketResponse<RemoteResult>, RemoteWebSocketRequest>(
                new RemoteWebSocketRequest<Guid>(_counter++, RemoteWebSocketRequestCode.Terminate, serverKey)
            )).Result;
        }
    }
}
