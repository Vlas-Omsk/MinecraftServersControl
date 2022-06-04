using System;
using Xunit;

namespace MinecraftServersControl.API.IntegrationTests
{
    [CollectionDefinition("ApiTests")]
    public sealed class ApiTestsCollectionFixture : ICollectionFixture<ApiTestsFixture>
    {
    }
}
