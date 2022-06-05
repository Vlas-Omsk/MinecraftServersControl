using MinecraftServersControl.API.Schema;
using MinecraftServersControl.API.Services;
using MinecraftServersControl.Logging;
using PinkJson2;
using PinkJson2.Serializers;
using System;
using System.IO;
using System.Text;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace MinecraftServersControl.API.WebSocket
{
    public sealed class MainHttpServer : HttpServer
    {
        private ApiContextFactory _apiContextFactory;
        private ILogger _logger;

        public MainHttpServer(ApiContextFactory apiContextFactory, ILogger logger, string url) : base(url)
        {
            _apiContextFactory = apiContextFactory;
            _logger = logger;

            OnRequest += OnRequestInternal;
        }

        private async void OnRequestInternal(object sender, HttpRequestEventArgs e)
        {
            switch (e.Request.HttpMethod)
            {
                case "POST":
                case "GET":
                    if (!TryResolvePath(e.Request, e.Response, out RequestCode code, out ApiService apiService))
                        return;

                    var dataType = Request.GetDataType(code);
                    object data = null;

                    if (dataType != null)
                    {
                        if (e.Request.HttpMethod == "GET")
                        {
                            SendHttpError(HttpStatusCode.BadRequest, e.Response);
                            return;
                        }

                        try
                        {
                            IJson json;

                            using (var streamReader = new StreamReader(e.Request.InputStream, e.Request.ContentEncoding))
                                json = Json.Parse(streamReader);

                            data = json.Deserialize(dataType, new ObjectSerializerOptions()
                            {
                                IgnoreMissingProperties = false
                            });
                        }
                        catch (Exception ex)
                        {
                            _logger.Warn(ex.ToString());
                            SendResponse(new Response(ResponseCode.DataError, ex.Message, null), e.Response);
                            return;
                        }
                    }

                    SendResponse(await apiService.ProcessAsync(new Request(code, data)), e.Response);
                    break;
                default:
                    SendHttpError(HttpStatusCode.MethodNotAllowed, e.Response);
                    break;
            }
        }

        private bool TryResolvePath(HttpListenerRequest httpRequest, HttpListenerResponse httpResponse, out RequestCode code, out ApiService apiService)
        {
            code = default;
            apiService = null;

            var segments = httpRequest.Url.Segments;

            if (segments.Length != 3)
            {
                SendHttpError(HttpStatusCode.BadRequest, httpResponse);
                return false;
            }

            if (!Enum.TryParse(segments[2].TrimEnd('/'), true, out code))
            {
                SendHttpError(HttpStatusCode.BadRequest, httpResponse);
                return false;
            }

            var apiServiceName = segments[1].TrimEnd('/');
            apiService = null;

            if (apiServiceName.Equals("user", StringComparison.OrdinalIgnoreCase))
            {
                apiService = _apiContextFactory.CreateApiService<UserApiService>();
            }
            else
            {
                SendHttpError(HttpStatusCode.BadRequest, httpResponse);
                return false;
            }

            if (!apiService.IsSupport(code))
            {
                SendHttpError(HttpStatusCode.BadRequest, httpResponse);
                return false;
            }

            return true;
        }

        private static void SendHttpError(HttpStatusCode statusCode, HttpListenerResponse httpResponse)
        {
            httpResponse.StatusCode = (int)statusCode;
            httpResponse.Close();
        }

        private static void SendResponse(Response response, HttpListenerResponse httpResponse)
        {
            var content = Encoding.UTF8.GetBytes(response.Serialize().ToString());

            httpResponse.ContentType = "application/json";
            httpResponse.ContentEncoding = Encoding.UTF8;
            httpResponse.ContentLength64 = content.Length;
            httpResponse.Close(content, true);
        }
    }
}
