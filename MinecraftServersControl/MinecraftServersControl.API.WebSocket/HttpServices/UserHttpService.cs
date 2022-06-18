using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;
using WebSocketSharp.Net;

namespace MinecraftServersControl.API.HttpServices
{
    public sealed class UserHttpService : HttpService
    {
        [HttpRequest(HttpMethod.Post, "/signin")]
        public async Task SignInAsync(UserDTO data)
        {
            SendSuccess(await Application.UserService.SignIn(data));
        }

        [HttpRequest(HttpMethod.Get, "/restore")]
        public async Task RestoreAsync()
        {
            var authorization = HttpRequest.Headers["Authorization"];

            if (authorization == null)
            {
                SendError(HttpStatusCode.Unauthorized);
                return;
            }

            var sessionId = Guid.Parse(authorization);
            var result = await Application.UserService.Restore(sessionId);

            SendSuccess(result);
        }
    }
}
