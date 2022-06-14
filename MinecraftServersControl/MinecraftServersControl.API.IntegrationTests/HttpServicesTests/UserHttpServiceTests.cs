using MinecraftServersControl.Common;
using MinecraftServersControl.Core;
using MinecraftServersControl.Core.DTO;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MinecraftServersControl.API.IntegrationTests
{
    [Collection("ApiTests")]
    public sealed class UserHttpServiceTests
    {
        private readonly ApiTestsFixture _fixture;

        public UserHttpServiceTests(ITestOutputHelper output, ApiTestsFixture testFixture)
        {
            _fixture = testFixture;
            _fixture.Output = output;
        }

        [Fact]
        public async Task SignIn_Should_Add_Session_To_Database()
        {
            var client = _fixture.CreateHttpClient();
            var response = await client.GetResponse<Result<SessionDTO>>("/user/signin", new UserDTO("Admin", "Admin".ToSha256Hash()));

            Assert.Equal(ResultCode.Success, response.Result.Code);
            using (var databaseContext = _fixture.DatabaseContextFactory.CreateDbContext())
                Assert.Contains(
                    databaseContext.Sessions.AsEnumerable(),
                    x => new Guid(x.Id) == response.Result.Data.SessionId &&
                    x.UserLogin == "Admin"
                );
        }

        [Fact]
        public async Task SignIn_Should_Return_UserNotFound_If_User_Is_Incorrect()
        {
            var client = _fixture.CreateHttpClient();
            var response = await client.GetResponse<Result<SessionDTO>>("/user/signin", new UserDTO("test", "test".ToSha256Hash()));

            Assert.Equal(ResultCode.UserNotFound, response.Result.Code);
        }

        [Fact]
        public async Task Restore_Should_Add_Session_To_Database_And_Remove_Current_Session_And_Close_Client()
        {
            var client = _fixture.CreateHttpClient();
            var response = await client.GetResponse<Result<SessionDTO>>("/user/signin", new UserDTO("Admin", "Admin".ToSha256Hash()));
            var response2 = await client.GetResponse<Result<SessionDTO>>("/user/restore", null, response.Result.Data.SessionId);

            Assert.Equal(ResultCode.Success, response2.Result.Code);
            using (var databaseContext = _fixture.DatabaseContextFactory.CreateDbContext())
            {
                Assert.DoesNotContain(
                    databaseContext.Sessions.AsEnumerable(),
                    x => new Guid(x.Id) == response.Result.Data.SessionId &&
                    x.UserLogin == "Admin"
                );
                Assert.Contains(
                    databaseContext.Sessions.AsEnumerable(),
                    x => new Guid(x.Id) == response2.Result.Data.SessionId &&
                    x.UserLogin == "Admin"
                );
            }
        }

        [Fact]
        public async Task Restore_Should_Return_SessionExpired_If_SessionId_Is_Incorrect()
        {
            var client = _fixture.CreateHttpClient();
            var response = await client.GetResponse<Result<SessionDTO>>("/user/restore", null, Guid.NewGuid());

            Assert.Equal(ResultCode.SessionExpired, response.Result.Code);
        }

        [Fact]
        public async Task Restore_Should_Return_SessionExpired_If_Session_Is_Expired()
        {
            var client = _fixture.CreateHttpClient();
            var response = await client.GetResponse<Result<SessionDTO>>("/user/signin", new UserDTO("Admin", "Admin".ToSha256Hash()));

            using (var databaseContext = _fixture.DatabaseContextFactory.CreateDbContext())
            {
                var session = await databaseContext.Sessions.FindAsync(response.Result.Data.SessionId.ToByteArray());
                session.ExpiresAt = (int)(DateTime.Now - TimeSpan.FromMinutes(1)).ToUnixTime();
                databaseContext.Sessions.Update(session);
                await databaseContext.SaveChangesAsync();
            }

            var response2 = await client.GetResponse<Result<SessionDTO>>("/user/restore", null, response.Result.Data.SessionId);

            Assert.Equal(ResultCode.SessionExpired, response2.Result.Code);
        }
    }
}
