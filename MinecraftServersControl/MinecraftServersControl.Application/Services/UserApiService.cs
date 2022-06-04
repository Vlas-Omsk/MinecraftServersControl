using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Services
{
    public sealed class UserApiService : ApiService
    {
        private AuthState _state;

        protected override async Task<bool> ProcessAsyncOverride(Request request)
        {
            if (_state != AuthState.Unauthorized)
            {
                await SendErrorAsync(request.Id, ResponseCode.InvalidState, null);
                return true;
            }

            switch (request.Code)
            {
                case RequestCode.SignIn:
                    await SignIn(request);
                    return true;
                case RequestCode.Restore:
                    await Restore(request);
                    return true;
            }

            return false;
        }

        private async Task SignIn(Request request)
        {
            var user = request.GetData<UserDTO>();
            var session = await Application.UserService.SignIn(user);

            await ProcessSession(request, session);
        }

        private async Task Restore(Request request)
        {
            var sessionId = request.GetData<Guid>();
            var session = await Application.UserService.Restore(sessionId);

            await ProcessSession(request, session);
        }

        private async Task ProcessSession(Request request, Result<SessionDTO> session)
        {
            _state = AuthState.Success;
            await SendSuccessAsync(request.Id, session);
            await CloseAsync();
        }
    }
}
