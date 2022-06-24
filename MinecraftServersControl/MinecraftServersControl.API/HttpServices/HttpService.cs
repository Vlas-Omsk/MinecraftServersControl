using MinecraftServersControl.Core.Abstractions;
using MinecraftServersControl.Logging;
using System;
using WebSocketSharp.Net;

namespace MinecraftServersControl.API.HttpServices
{
    public abstract class HttpService
    {
        protected IApplication Application { get; private set; }
        protected Logger Logger { get; private set; }
        protected HttpListenerRequest HttpRequest { get; private set; }
        protected HttpListenerResponse HttpResponse { get; private set; }

        private HttpRequestHandler _requestHandler;

        internal void Init(IApplication application, Logger logger, HttpListenerRequest httpRequest, HttpListenerResponse httpResponse, HttpRequestHandler requestHandler)
        {
            Application = application;
            Logger = logger;
            HttpRequest = httpRequest;
            HttpResponse = httpResponse;
            _requestHandler = requestHandler;
        }

        public void SendSuccess<T>(T data)
        {
            _requestHandler.SendSuccess(data);
        }

        public void SendError(HttpStatusCode statusCode)
        {
            _requestHandler.SendError(statusCode);
        }

        public void SendError(HttpStatusCode statusCode, string errorMessage)
        {
            _requestHandler.SendError(statusCode, errorMessage);
        }
    }
}
