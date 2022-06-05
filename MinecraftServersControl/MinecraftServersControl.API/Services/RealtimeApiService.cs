using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core;
using MinecraftServersControl.Logging;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Services
{
    public abstract class RealtimeApiService
    {
        protected ILogger Logger { get; private set; }
        protected Application Application { get; private set; }

        private IClient _client;

        internal RealtimeApiService()
        {
        }

        internal void Init(IClient client, ILogger logger, Application application)
        {
            Logger = logger;
            Application = application;
            _client = client;
        }

        public Task OpenAsync()
        {
            Logger.Info("Opened " + _client.GetInfo());

            return OpenOverrideAsync();
        }

        protected virtual Task OpenOverrideAsync()
        {
            return Task.CompletedTask;
        }

        public Task CloseAsync()
        {
            Logger.Info("Closed " + _client.GetInfo());

            return CloseOverrideAsync();
        }

        protected virtual Task CloseOverrideAsync()
        {
            return Task.CompletedTask;
        }

        public async Task ProcessAsync(Request request)
        {
            Logger.Info($"Request: {request.Code}, Client: {_client.GetInfo()}");

            try
            {
                if (await ProcessOverrideAsync(request))
                    return;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                await SendResponse(request, Response.CreateError(ResponseCode.InternalServerError, null));
                return;
            }

            await SendResponse(request, Response.CreateError(ResponseCode.InvalidCode, null));
        }

        protected abstract Task<bool> ProcessOverrideAsync(Request request);

        protected Task SendResponse(Request targetRequest, Response response)
        {
            Logger.Info($"Response: {response.Code}, Client: {_client.GetInfo()}");

            return _client.SendResponseAsync(targetRequest, response);
        }

        protected Task RequestCloseAsync()
        {
            return _client.CloseAsync();
        }
    }
}
