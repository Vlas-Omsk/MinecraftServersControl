using MinecraftServersControl.API.HttpServices;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Logging;
using System;
using System.Collections.Generic;
using WebSocketSharp.Server;

namespace MinecraftServersControl.API
{
    public sealed class ApiHttpServer : HttpServer
    {
        private readonly IApplication _application;
        private readonly Logger _logger;
        private readonly Dictionary<Uri, Type> _httpServices = new Dictionary<Uri, Type>();

        public ApiHttpServer(IApplication application, Logger logger, string url) : base(url)
        {
            _application = application;
            _logger = logger;

            OnRequest += OnRequestInternal;
        }

        private void OnRequestInternal(object sender, HttpRequestEventArgs e)
        {
            var httpRequestHandler = new HttpRequestHandler(e.Request, e.Response, _application, _logger, _httpServices);
            httpRequestHandler.ProcessRequest();
        }

        public void AddHttpService<T>(string path) where T : HttpService, new()
        {
            _httpServices.Add(new Uri(path, UriKind.RelativeOrAbsolute), typeof(T));
        }
    }
}
