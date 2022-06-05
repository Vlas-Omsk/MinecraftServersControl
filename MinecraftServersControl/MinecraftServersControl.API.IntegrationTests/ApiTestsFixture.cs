using MinecraftServersControl.API.IntegrationTests.Helpers;
using MinecraftServersControl.API.Services;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;
using Xunit.Abstractions;

namespace MinecraftServersControl.API.IntegrationTests
{
    public sealed class ApiTestsFixture : IDisposable
    {
        public ITestOutputHelper Output { get; set; }
        public ApiContextFactory ApiContextFactory { get; }
        public DevDatabaseContextFactory DatabaseContextFactory { get; }

        public ApiTestsFixture()
        {
            var logger = new CallbackLogger(x => Output.WriteLine(x));
            DatabaseContextFactory = new DevDatabaseContextFactory();
            ApiContextFactory = new ApiContextFactory(DatabaseContextFactory, logger);
        }

        public ApiClient<T> CreateClient<T>() where T : RealtimeApiService, new()
        {
            return new ApiClient<T>(ApiContextFactory);
        }

        public void Dispose()
        {
            DatabaseContextFactory.Dispose();
        }
    }
}
