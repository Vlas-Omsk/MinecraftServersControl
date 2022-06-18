using MinecraftServersControl.API.IntegrationTests.Helpers;
using MinecraftServersControl.Core;
using MinecraftServersControl.Core.Abstractions;
using MinecraftServersControl.DAL;
using MinecraftServersControl.Logging;
using System;
using Xunit.Abstractions;

namespace MinecraftServersControl.API.IntegrationTests
{
    public sealed class ApiTestsFixture : IDisposable
    {
        public ITestOutputHelper Output { get; set; }
        public IApplication Application { get; }
        public DevDatabaseContextFactory DatabaseContextFactory { get; }

        public ApiTestsFixture()
        {
            var logger = new CallbackLogger(x => Output?.WriteLine(x));
            DatabaseContextFactory = new DevDatabaseContextFactory();
            Application = new Application(DatabaseContextFactory, logger);
            var server = new ApiServer(Application, logger, "http://127.0.0.1:8888");
            server.Start();
        }

        public WebSocketClient CreateWebSocketClient()
        {
            return new WebSocketClient("ws://127.0.0.1:8888/gateway");
        }

        public HttpClient CreateHttpClient()
        {
            return new HttpClient("http://127.0.0.1:8888/");
        }

        public void Dispose()
        {
            DatabaseContextFactory.Dispose();
        }
    }
}
