using MinecraftServersControl.API.Schema;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Services
{
    public sealed class ServerApiService : ApiService
    {
        private AuthState _state;
        private Guid _sessionId;

        protected override async Task<bool> ProcessAsyncOverride(Request request)
        {
            switch (request.Code)
            {
                case RequestCode.Auth:
                    await Auth(request);
                    return true;
            }

            if (_state == AuthState.Unauthorized)
            {
                await SendErrorAsync(request.Id, ResponseCode.InvalidState, null);
                return true;
            }

            switch (request.Code)
            {
                case RequestCode.GetServers:
                    await GetServers(request);
                    return true;
            }

            return false;
        }

        private async Task Auth(Request request)
        {
            if (_state != AuthState.Unauthorized)
            {
                await SendErrorAsync(request.Id, ResponseCode.InvalidState, null);
                return;
            }

            var sessionId = request.GetData<Guid>();
            var result = await Application.UserService.VerifySession(sessionId);

            await SendSuccessAsync(request.Id, result);

            if (result.HasErrors())
                return;

            _sessionId = sessionId;
            _state = AuthState.Success;
            Application.UserService.SessionRemoved += OnAuthServiceSessionRemoved;
        }

        private async void OnAuthServiceSessionRemoved(object sender, Core.DTO.Result<Guid> e)
        {
            await SendSuccessAsync(Response.BroadcastRequestId, e);
            await CloseAsync();
        }

        private async Task GetServers(Request request)
        {
            var result = await Application.ServerService.GetServers(_sessionId);

            await SendSuccessAsync(request.Id, result);
        }

        public override ValueTask DisposeAsync()
        {
            Application.UserService.SessionRemoved -= OnAuthServiceSessionRemoved;

            return base.DisposeAsync();
        }
    }
}
