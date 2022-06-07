using MinecraftServersControl.Core;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Logging;
using System;
using WebSocketSharp.Net;

namespace MinecraftServersControl.API.HttpServices
{
    public abstract class HttpService
    {
        protected Application Application { get; private set; }
        protected ILogger Logger { get; private set; }
        protected HttpListenerRequest HttpRequest { get; private set; }
        protected HttpListenerResponse HttpResponse { get; private set; }

        internal void Init(Application application, ILogger logger, HttpListenerRequest httpRequest, HttpListenerResponse httpResponse)
        {
            Application = application;
            Logger = logger;
            HttpRequest = httpRequest;
            HttpResponse = httpResponse;
        }

        protected void SendSuccess()
        {
            Logger.Info($"Response: OK, Url: {HttpRequest.Url}, Client: {HttpRequest.RemoteEndPoint}");

            HttpResponse.SendSuccess();
        }

        protected void SendSuccess<T>(Result<T> data)
        {
            Logger.Info($"Response: OK, Result: {data.Code}, Url: {HttpRequest.Url}, Client: {HttpRequest.RemoteEndPoint}");

            HttpResponse.SendSuccess(data);
        }

        protected void SendError(HttpStatusCode statusCode)
        {
            Logger.Info($"Response: {statusCode}, Url: {HttpRequest.Url}, Client: {HttpRequest.RemoteEndPoint}");

            HttpResponse.SendError(statusCode);
        }
    }
}
