using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core;
using MinecraftServersControl.Logging;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.Services
{
    public abstract class ApiService
    {
        protected ILogger Logger { get; private set; }
        protected Application Application { get; private set; }

        internal ApiService()
        {
        }

        internal void Init(ILogger logger, Application application)
        {
            Logger = logger;
            Application = application;
        }

        public abstract bool IsSupport(RequestCode requestCode);

        public async Task<Response> ProcessAsync(Request request)
        {
            Logger.Info($"Request: {request.Code}");

            try
            {
                var response = await ProcessOverrideAsync(request);
                if (response != null)
                {
                    Logger.Info($"Response: {response.Code}");
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return Response.CreateError(ResponseCode.InternalServerError, null);
            }

            return Response.CreateError(ResponseCode.InvalidCode, null);
        }

        protected abstract Task<Response> ProcessOverrideAsync(Request request);
    }
}
