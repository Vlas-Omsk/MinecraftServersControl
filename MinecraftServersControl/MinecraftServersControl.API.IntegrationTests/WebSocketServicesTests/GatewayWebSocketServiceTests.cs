using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core;
using MinecraftServersControl.Core.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MinecraftServersControl.API.IntegrationTests
{
    [Collection("ApiTests")]
    public sealed class GatewayWebSocketServiceTests
    {
        private readonly ApiTestsFixture _fixture;

        public GatewayWebSocketServiceTests(ITestOutputHelper output, ApiTestsFixture testFixture)
        {
            _fixture = testFixture;
            _fixture.Output = output;
        }

        [Fact]
        public async Task Should_Return_AuthorizationFromAnotherPlace_If_Restore_Is_Made_On_The_Same_Token()
        {
            var client = _fixture.CreateHttpClient();
            var response = await client.GetResponse<Result<SessionDTO>>("/user/signin", new UserDTO("Admin", "Admin".ToSha256Hash()));

            var client2 = _fixture.CreateWebSocketClient();
            var response2 = await client2.GetResponse(WebSocketRequestCode.Auth, response.Result.Data.SessionId);

            var task = client2.GetBroadcastResult<Result<Guid>>(ResultCode.AuthorizationFromAnotherPlace);

            var client3 = _fixture.CreateHttpClient();
            var response3 = await client3.GetResponse<Result<SessionDTO>>("/user/restore", null, response.Result.Data.SessionId);

            await task;

            Assert.True(client2.Closed);
            Assert.Equal(ResultCode.AuthorizationFromAnotherPlace, task.Result.Result.Code);
        }

        [Fact]
        public async Task Should_Return_InvalidState_If_Authorization_Was_Not_Performed()
        {
            var client = _fixture.CreateWebSocketClient();
            var response = await client.GetResponse<Result<ComputerDTO>>(WebSocketRequestCode.GetServers);

            Assert.Equal(WebSocketResponseCode.InvalidState, response.Code);
        }

        [Fact]
        public async Task Auth_Should_Change_State()
        {
            var client = _fixture.CreateHttpClient();
            var response = await client.GetResponse<Result<SessionDTO>>("/user/signin", new UserDTO("Admin", "Admin".ToSha256Hash()));

            var client2 = _fixture.CreateWebSocketClient();
            var result2 = await client2.GetResponse(WebSocketRequestCode.Auth, response.Result.Data.SessionId);

            Assert.Equal(ResultCode.Success, result2.Result.Code);
        }

        [Fact]
        public async Task Auth_Should_Return_SessionExpired_If_SessionId_Is_Expired()
        {
            var client = _fixture.CreateHttpClient();
            var response = await client.GetResponse<Result<SessionDTO>>("/user/signin", new UserDTO("Admin", "Admin".ToSha256Hash()));

            using (var databaseContext = _fixture.DatabaseContextFactory.CreateDbContext())
            {
                var session = await databaseContext.Sessions.FindAsync(response.Result.Data.SessionId.ToByteArray());
                session.ExpiresAt = (int)(DateTime.Now.AddDays(-1).ToUnixTime());
                databaseContext.Update(session);
                await databaseContext.SaveChangesAsync();
            }

            var client2 = _fixture.CreateWebSocketClient();
            var response2 = await client2.GetResponse(WebSocketRequestCode.Auth, response.Result.Data.SessionId);

            Assert.Equal(ResultCode.SessionExpired, response2.Result.Code);
        }

        [Fact]
        // FIXME: remote server
        public async Task GetServers_Should_Return_ComputerDTO_Array()
        {
            var client = _fixture.CreateHttpClient();
            var response = await client.GetResponse<Result<SessionDTO>>("/user/signin", new UserDTO("Admin", "Admin".ToSha256Hash()));

            var client2 = _fixture.CreateWebSocketClient();
            var response2 = await client2.GetResponse(WebSocketRequestCode.Auth, response.Result.Data.SessionId);
            var response3 = await client2.GetResponse<Result<IEnumerable<ComputerDTO>>>(WebSocketRequestCode.GetServers);

            Assert.Equal(ResultCode.Success, response3.Result.Code);
            Assert.NotNull(response3.Result.Data);
        }
    }
}
