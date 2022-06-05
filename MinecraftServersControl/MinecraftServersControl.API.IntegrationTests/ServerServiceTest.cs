using MinecraftServersControl.API.Schema;
using MinecraftServersControl.API.Services;
using MinecraftServersControl.Core.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MinecraftServersControl.API.IntegrationTests
{
    [Collection("ApiTests")]
    public sealed class ServerServiceTest
    {
        private readonly ApiTestsFixture _fixture;

        public ServerServiceTest(ITestOutputHelper output, ApiTestsFixture testFixture)
        {
            _fixture = testFixture;
            _fixture.Output = output;
        }

        [Fact]
        public async Task Should_Return_InvalidState_If_Authorization_Was_Not_Performed()
        {
            var client = _fixture.CreateClient<GatewayApiService>();
            var response = await client.GetResponse(RequestCode.GetServers, null);

            Assert.Equal(ResponseCode.InvalidState, response.Code);
        }

        [Fact]
        public async Task Auth_Should_Change_State()
        {
            var client = _fixture.CreateClient<UserApiService>();
            var result = await client.GetResult<SessionDTO>(RequestCode.SignIn, new UserDTO("Admin", "Admin".ToSha256Hash()));

            var client2 = _fixture.CreateClient<GatewayApiService>();
            var result2 = await client2.GetResult(RequestCode.Auth, result.Data.SessionId);

            Assert.Equal(ResultCode.Success, result2.Code);
        }

        [Fact]
        public async Task Auth_Should_Return_SessionExpired_If_SessionId_Is_Expired()
        {
            var client = _fixture.CreateClient<GatewayApiService>();
            var result = await client.GetResult(RequestCode.Auth, Guid.NewGuid());

            Assert.Equal(ResultCode.SessionExpired, result.Code);
        }

        [Fact]
        public async Task GetServers_Should_Return_ComputerDTO_Array()
        {
            var client = _fixture.CreateClient<UserApiService>();
            var result = await client.GetResult<SessionDTO>(RequestCode.SignIn, new UserDTO("Admin", "Admin".ToSha256Hash()));

            var client2 = _fixture.CreateClient<GatewayApiService>();
            await client2.GetResult(RequestCode.Auth, result.Data.SessionId);

            var result3 = await client2.GetResult<IEnumerable<ComputerDTO>>(RequestCode.GetServers, null);

            Assert.Equal(ResultCode.Success, result3.Code);
            Assert.NotNull(result3.Data);
        }
    }
}
