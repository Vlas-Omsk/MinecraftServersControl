using MinecraftServersControl.API.Schema;
using System;

namespace MinecraftServersControl.API.IntegrationTests
{
    public sealed class ApiException : Exception
    {
        public Response Response { get; }

        public ApiException(Response response) : base($"{response.Code}: {response.Code}")
        {
            Response = response;
        }
    }
}
