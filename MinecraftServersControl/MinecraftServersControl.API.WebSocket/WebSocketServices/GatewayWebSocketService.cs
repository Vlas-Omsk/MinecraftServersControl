using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace MinecraftServersControl.API.WebSocketServices
{
    public sealed class GatewayWebSocketService : WebSocketService
    {
        private AuthState _state;
        private Guid _sessionId;

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);

            Application.UserService.SessionRemoved -= OnSessionRemoved;
            Application.ComputerService.ComputerStarted -= OnComputerStarted;
            Application.ComputerService.ComputerStopped -= OnComputerStopped;
            Application.ServerService.ServerStarted -= OnServerStarted;
            Application.ServerService.ServerStopped -= OnServerStopped;
            Application.ServerService.ServerOutput -= OnServerOutput;
        }

        [WebSocketRequest(WebSocketRequestCode.Auth)]
        public async Task AuthAsync(WebSocketRequest<Guid> request)
        {
            if (!VerifyState(request, AuthState.Unauthorized))
                return;

            var result = await Application.UserService.VerifySession(request.Data);

            SendSuccess(request.Id, result);

            if (result.HasErrors())
                return;

            _sessionId = request.Data;
            _state = AuthState.Success;

            Application.UserService.SessionRemoved += OnSessionRemoved;
            Application.ComputerService.ComputerStarted += OnComputerStarted;
            Application.ComputerService.ComputerStopped += OnComputerStopped;
            Application.ServerService.ServerStarted += OnServerStarted;
            Application.ServerService.ServerStopped += OnServerStopped;
            Application.ServerService.ServerOutput += OnServerOutput;
        }

        private async void OnSessionRemoved(object sender, Result<Guid> e)
        {
            await Task.Run(() =>
            {
                if (_sessionId != e.Data)
                    return;

                SendSuccess(WebSocketResponse.BroadcastRequestId, e);
                Close();
            });
        }

        [WebSocketRequest(WebSocketRequestCode.GetServers)]
        public async Task GetServersAsync(WebSocketRequest request)
        {
            if (!VerifyState(request, AuthState.Success))
                return;

            var result = await Application.ServerService.GetServers();

            SendSuccess(request.Id, result);
        }

        [WebSocketRequest(WebSocketRequestCode.GetOutput)]
        public async Task GetOutputAsync(WebSocketRequest<TargetServerDTO> request)
        {
            if (!VerifyState(request, AuthState.Success))
                return;

            var result = await Application.ServerService.GetOutput(request.Data);

            SendSuccess(request.Id, result);
        }

        [WebSocketRequest(WebSocketRequestCode.Input)]
        public async Task InputAsync(WebSocketRequest<ServerInputDTO> request)
        {
            if (!VerifyState(request, AuthState.Success))
                return;

            var result = await Application.ServerService.Input(request.Data);

            SendSuccess(request.Id, result);
        }

        [WebSocketRequest(WebSocketRequestCode.StartServer)]
        public async Task StartServerAsync(WebSocketRequest<TargetServerDTO> request)
        {
            if (!VerifyState(request, AuthState.Success))
                return;

            var result = await Application.ServerService.Start(request.Data);

            SendSuccess(request.Id, result);
        }

        [WebSocketRequest(WebSocketRequestCode.TerminateServer)]
        public async Task TerminateServerAsync(WebSocketRequest<TargetServerDTO> request)
        {
            if (!VerifyState(request, AuthState.Success))
                return;

            var result = await Application.ServerService.Terminate(request.Data);

            SendSuccess(request.Id, result);
        }

        private bool VerifyState(WebSocketRequest request, AuthState authState)
        {
            if (_state != authState)
            {
                SendError(request.Id, WebSocketResponseCode.InvalidState, null);
                return false;
            }

            return true;
        }

        private async void OnComputerStarted(object sender, Result<ComputerStateDTO> e)
        {
            await Task.Run(() => SendSuccess(WebSocketResponse.BroadcastRequestId, e));
        }

        private async void OnComputerStopped(object sender, Result<ComputerStateDTO> e)
        {
            await Task.Run(() => SendSuccess(WebSocketResponse.BroadcastRequestId, e));
        }

        private async void OnServerStarted(object sender, Result<TargetServerDTO> e)
        {
            await Task.Run(() => SendSuccess(WebSocketResponse.BroadcastRequestId, e));
        }

        private async void OnServerStopped(object sender, Result<TargetServerDTO> e)
        {
            await Task.Run(() => SendSuccess(WebSocketResponse.BroadcastRequestId, e));
        }

        private async void OnServerOutput(object sender, Result<ServerOutputDTO> e)
        {
            await Task.Run(() => SendSuccess(WebSocketResponse.BroadcastRequestId, e));
        }
    }
}
