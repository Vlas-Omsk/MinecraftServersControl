using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Services
{
    public sealed class UserApiService : ApiService
    {
        public override bool IsSupport(RequestCode requestCode)
        {
            switch (requestCode)
            {
                case RequestCode.SignIn:
                case RequestCode.Restore:
                    return true;
                default:
                    return false;
            }
        }

        protected override async Task<Response> ProcessOverrideAsync(Request request)
        {
            switch (request.Code)
            {
                case RequestCode.SignIn:
                    return await SignIn(request);
                case RequestCode.Restore:
                    return await Restore(request);
            }

            return null;
        }

        private async Task<Response> SignIn(Request request)
        {
            var user = request.GetData<UserDTO>();
            var session = await Application.UserService.SignIn(user);

            return Response.CreateSuccess(session);
        }

        private async Task<Response> Restore(Request request)
        {
            var sessionId = request.GetData<Guid>();
            var session = await Application.UserService.Restore(sessionId);

            return Response.CreateSuccess(session);
        }
    }
}
