using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Logging;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Services
{
    public abstract class ApiService : IDisposable, IAsyncDisposable
    {
        protected IClient Client { get; private set; }
        protected ILogger Logger { get; private set; }
        protected Application Application { get; private set; }

        internal ApiService()
        {
        }

        internal void Init(IClient client, ILogger logger, Application application)
        {
            Client = client;
            Logger = logger;
            Application = application;
        }

        public Task OnConnectedAsync()
        {
            Logger.Info("Connected " + Client.GetInfo());

            return Task.CompletedTask;
        }

        public async Task ProcessAsync(Request request)
        {
            Logger.Info($"Client: {Client.GetInfo()}, Request: {request.Code}, Id: {request.Id}");

            try
            {
                if (await ProcessAsyncOverride(request))
                    return;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                await SendErrorAsync(request.Id, ResponseCode.InternalServerError, null);
            }

            await SendErrorAsync(request.Id, ResponseCode.InvalidCode, null);
        }

        protected abstract Task<bool> ProcessAsyncOverride(Request request);

        protected Task SendErrorAsync(int requestId, ResponseCode code, string message)
        {
            return SendResponse(new Response(requestId, code, message, null));
        }

        protected Task SendSuccessAsync(int requestId, Result result)
        {
            return SendResponse(new Response(requestId, ResponseCode.Success, null, result));
        }

        protected Task SendResponse(Response response)
        {
            Logger.Info($"Client: {Client.GetInfo()}, Response: {response.Code}, Id: {response.RequestId}");

            return Client.SendResponseAsync(response);
        }

        protected Task CloseAsync()
        {
            return Client.CloseAsync();
        }

        public void Dispose()
        {
            DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public virtual ValueTask DisposeAsync()
        {
            Logger.Info("Disconnected " + Client.GetInfo());

            return ValueTask.CompletedTask;
        }
    }
}
