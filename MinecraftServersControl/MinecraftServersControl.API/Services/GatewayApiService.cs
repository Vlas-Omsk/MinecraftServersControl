using MinecraftServersControl.API.Schema;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Services
{
    public sealed class GatewayApiService : RealtimeApiService
    {
        private AuthState _state;
        private Guid _sessionId;

        protected override async Task<bool> ProcessOverrideAsync(Request request)
        {
            switch (request.Code)
            {
                case RequestCode.Auth:
                    await Auth(request);
                    return true;
            }

            if (_state == AuthState.Unauthorized)
            {
                await SendResponse(request, Response.CreateError(ResponseCode.InvalidState, null));
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
                await SendResponse(request, Response.CreateError(ResponseCode.InvalidState, null));
                return;
            }

            var sessionId = request.GetData<Guid>();
            var result = await Application.UserService.VerifySession(sessionId);

            await SendResponse(request, Response.CreateSuccess(result));

            if (result.HasErrors())
                return;

            _sessionId = sessionId;
            _state = AuthState.Success;
            Application.UserService.SessionRemoved += OnSessionRemoved;
        }

        private async void OnSessionRemoved(object sender, Core.DTO.Result<Guid> e)
        {
            if (_sessionId != e.Data)
                return;

            await SendResponse(null, Response.CreateSuccess(e));
            await CloseAsync();
        }

        private async Task GetServers(Request request)
        {
            var result = await Application.ServerService.GetServers(_sessionId);

            await SendResponse(request, Response.CreateSuccess(result));
        }

        protected override Task CloseOverrideAsync()
        {
            Application.UserService.SessionRemoved -= OnSessionRemoved;

            return base.CloseOverrideAsync();
        }
    }
}
