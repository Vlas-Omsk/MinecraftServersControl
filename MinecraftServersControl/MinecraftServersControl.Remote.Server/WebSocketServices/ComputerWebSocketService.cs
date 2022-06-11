using MinecraftServersControl.Core;
using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinecraftServersControl.Remote.Server.WebSocketServices
{
    public sealed class ComputerWebSocketService : WebSocketService, INetworkComputer
    {
        public Guid ComputerKey { get; private set; }

        private int _counter;

        private bool IsAuthorized => ComputerKey != default;

        [WebSocketResponse(ResultCode.Verify)]
        public async Task VerifyAsync(WebSocketResponse<Result<Guid>> response)
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
        }

        [WebSocketResponse(ResultCode.ServerStarted)]
        public async Task ServerStartedAsync(WebSocketResponse<Result<Guid>> response)
        {
            if (!IsAuthorized)
                return;

            await Application.ServerService.RaiseServerStarted(ComputerKey, response.Result.Data);
        }

        [WebSocketResponse(ResultCode.ServerStopped)]
        public async Task ServerStoppedAsync(WebSocketResponse<Result<Guid>> response)
        {
            if (!IsAuthorized)
                return;

            await Application.ServerService.RaiseServerStopped(ComputerKey, response.Result.Data);
        }

        public async Task<Result<IEnumerable<ServerDTO>>> GetInfo(Guid computerKey)
        {
            var response = await GetResponse<WebSocketResponse<Result<IEnumerable<ServerDTO>>>, WebSocketRequest>(new WebSocketRequest(_counter++, WebSocketRequestCode.GetInfo));

            return response.Result;
        }

        public Task<Result<string>> GetOutput(Guid computerKey, Guid serverKey)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Start(Guid computerKey, Guid serverKey)
        {
            throw new NotImplementedException();
        }

        public Task<Result> Terminate(Guid computerKey, Guid serverKey)
        {
            throw new NotImplementedException();
        }
    }
}
