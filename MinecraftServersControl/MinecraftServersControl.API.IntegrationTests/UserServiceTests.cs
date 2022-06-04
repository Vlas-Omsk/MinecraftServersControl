using MinecraftServersControl.API.Schema;
using MinecraftServersControl.API.Services;
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
    public sealed class UserServiceTests
    {
        private readonly ApiTestsFixture _fixture;

        public UserServiceTests(ITestOutputHelper output, ApiTestsFixture testFixture)
        {
            _fixture = testFixture;
            _fixture.Output = output;
        }

        [Fact]
        public async Task SignIn_Should_Add_Session_To_Database_And_Close_Client()
        {
            var client = _fixture.CreateClient<UserApiService>();
            var result = await client.GetResult<SessionDTO>(RequestCode.SignIn, new UserDTO("Admin", "Admin".ToSha256Hash()));

            Assert.Equal(ResultCode.Success, result.Code);
            using (var databaseContext = _fixture.DatabaseContextFactory.CreateDbContext())
                Assert.Contains(
                    databaseContext.Sessions.AsEnumerable(), 
                    x => new Guid(x.Id) == result.Data.SessionId && 
                    x.UserLogin == "Admin"
                );
            Assert.True(client.Closed);
        }

        [Fact]
        public async Task SignIn_Should_Return_UserNotFound_If_User_Is_Incorrect()
        {
            var client = _fixture.CreateClient<UserApiService>();
            var result = await client.GetResult<SessionDTO>(RequestCode.SignIn, new UserDTO("test", "test".ToSha256Hash()));

            Assert.Equal(ResultCode.UserNotFound, result.Code);
        }

        [Fact]
        public async Task Restore_Should_Add_Session_To_Database_And_Remove_Current_Session_And_Close_Client()
        {
            var client = _fixture.CreateClient<UserApiService>();
            var result = await client.GetResult<SessionDTO>(RequestCode.SignIn, new UserDTO("Admin", "Admin".ToSha256Hash()));

            var client2 = _fixture.CreateClient<UserApiService>();
            var result2 = await client2.GetResult<SessionDTO>(RequestCode.Restore, result.Data.SessionId);

            Assert.Equal(ResultCode.Success, result2.Code);
            using (var databaseContext = _fixture.DatabaseContextFactory.CreateDbContext())
            {
                Assert.DoesNotContain(
                    databaseContext.Sessions.AsEnumerable(),
                    x => new Guid(x.Id) == result.Data.SessionId &&
                    x.UserLogin == "Admin"
                );
                Assert.Contains(
                    databaseContext.Sessions.AsEnumerable(),
                    x => new Guid(x.Id) == result2.Data.SessionId &&
                    x.UserLogin == "Admin"
                );
            }
            Assert.True(client.Closed);
        }

        [Fact]
        public async Task Restore_Should_Return_SessionExpired_If_SessionId_Is_Incorrect()
        {
            var client = _fixture.CreateClient<UserApiService>();
            var result = await client.GetResult<SessionDTO>(RequestCode.Restore, Guid.NewGuid());

            Assert.Equal(ResultCode.SessionExpired, result.Code);
        }

        [Fact]
        public async Task Restore_Should_Return_SessionExpired_If_Session_Is_Expired()
        {
            var client = _fixture.CreateClient<UserApiService>();
            var result = await client.GetResult<SessionDTO>(RequestCode.SignIn, new UserDTO("Admin", "Admin".ToSha256Hash()));

            using (var databaseContext = _fixture.DatabaseContextFactory.CreateDbContext())
            {
                var session = await databaseContext.Sessions.FindAsync(result.Data.SessionId.ToByteArray());
                session.ExpiresAt = (int)(DateTime.Now - TimeSpan.FromMinutes(1)).ToUnixTime();
                databaseContext.Sessions.Update(session);
                await databaseContext.SaveChangesAsync();
            }

            var client2 = _fixture.CreateClient<UserApiService>();
            var result2 = await client2.GetResult<SessionDTO>(RequestCode.Restore, result.Data.SessionId);

            Assert.Equal(ResultCode.SessionExpired, result2.Code);
        }
    }
}
