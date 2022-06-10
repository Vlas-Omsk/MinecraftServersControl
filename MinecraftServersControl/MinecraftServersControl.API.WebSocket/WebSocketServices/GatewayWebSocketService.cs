using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core;
using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.WebSocketServices
{
    public sealed class GatewayWebSocketService : WebSocketService
    {
        private AuthState _state;
        private Guid _sessionId;

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

            var result = await Application.ServerService.GetServers(_sessionId);

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
    }
}
