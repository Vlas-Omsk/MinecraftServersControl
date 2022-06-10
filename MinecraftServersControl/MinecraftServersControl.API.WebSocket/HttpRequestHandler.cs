using MinecraftServersControl.API.HttpServices;
using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Logging;
using PinkJson2;
using PinkJson2.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Net;

namespace MinecraftServersControl.API
{
    public sealed class HttpRequestHandler
    {
        private HttpListenerRequest _httpRequest;
        private HttpListenerResponse _httpResponse;
        private readonly Application _application;
        private readonly ILogger _logger;
        private readonly Dictionary<Uri, Type> _httpServices = new Dictionary<Uri, Type>();
        private bool _handled;

        internal HttpRequestHandler(HttpListenerRequest httpRequest, HttpListenerResponse httpResponse, Application application, ILogger logger, Dictionary<Uri, Type> httpServices)
        {
            _httpRequest = httpRequest;
            _httpResponse = httpResponse;
            _application = application;
            _logger = logger;
            _httpServices = httpServices;
        }

        public void SendSuccess<T>(Result<T> data)
        {
            Send(HttpStatusCode.OK, new HttpResponse<Result<T>>(data));
        }

        public void SendError(HttpStatusCode statusCode)
        {
            SendError(statusCode, null);
        }

        public void SendError(HttpStatusCode statusCode, string errorMessage)
        {
            Send(statusCode, new HttpResponse(errorMessage));
        }

        private void Send(HttpStatusCode statusCode, HttpResponse response)
        {
            if (_handled)
                throw new Exception("Handled");

            _handled = true;

            _logger.Info($"Url: {_httpRequest.Url}, Response: {response}, Client: {_httpRequest.RemoteEndPoint}");

            var content = Encoding.UTF8.GetBytes(response.Serialize().ToString());

            _httpResponse.StatusCode = (int)statusCode;
            _httpResponse.ContentType = ContentTypeNames.ApplicationJson;
            _httpResponse.ContentEncoding = Encoding.UTF8;
            _httpResponse.ContentLength64 = content.Length;
            _httpResponse.Close(content, true);
        }

        internal void ProcessRequest()
        {
            if (!Enum.TryParse(_httpRequest.HttpMethod, true, out HttpMethod httpMethod))
            {
                SendError(HttpStatusCode.MethodNotAllowed);
                return;
            }

            _httpResponse.SetHeader("Access-Control-Allow-Origin", "*");
            _httpResponse.SetHeader("Access-Control-Allow-Headers", "*");

            if (httpMethod == HttpMethod.Options)
            {
                _httpResponse.Close();
                return;
            }

            if (httpMethod == HttpMethod.Post && _httpRequest.ContentType != ContentTypeNames.ApplicationJson)
            {
                SendError(HttpStatusCode.BadRequest);
                return;
            }

            var resolvedUrl = TryResolveUrl(httpMethod, _httpRequest.Url);

            if (resolvedUrl == null)
            {
                SendError(HttpStatusCode.BadRequest);
                return;
            }

            _logger.Info($"HttpMethod: {httpMethod}, Url: {_httpRequest.Url}, Request: {resolvedUrl.ServiceType.Name}.{resolvedUrl.Method.Name}, Client: {_httpRequest.RemoteEndPoint}");

            try
            {
                var parameters = MapParameters(resolvedUrl.Method, resolvedUrl.CurrentSegments, resolvedUrl.TargetSegments, _httpRequest.InputStream, _httpRequest.ContentEncoding);

                var objCctor = resolvedUrl.ServiceType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, Type.EmptyTypes, null);
                var obj = (HttpService)objCctor.Invoke(null);

                obj.Init(_application, _logger, _httpRequest, _httpResponse, this);

                var methodResult = resolvedUrl.Method.Invoke(obj, parameters);

                if (methodResult is Task task)
                    task.ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                SendError(HttpStatusCode.InternalServerError, ex.Message);
                return;
            }

            if (!_handled)
                SendError(HttpStatusCode.NotImplemented);
        }

        private ResolvedUrl TryResolveUrl(HttpMethod httpMethod, Uri url)
        {
            var currentSegments = GetSegments(url).ToArray();

            foreach (var httpService in _httpServices)
            {
                var targetSegments = GetTargetSegments(httpService.Key).ToArray();

                if (!CompareUrls(currentSegments, targetSegments))
                    continue;

                var methods = httpService.Value
                    .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(x => (method: x, httpRequestAttribute: x.GetCustomAttribute<HttpRequest>()))
                    .Where(x => x.httpRequestAttribute != null && x.httpRequestAttribute.HttpMethod == httpMethod);

                foreach (var method in methods)
                {
                    targetSegments = GetTargetSegments(new Uri(httpService.Key + method.httpRequestAttribute.Path, UriKind.RelativeOrAbsolute)).ToArray();

                    if (!TryResolveUrlForMethod(currentSegments, targetSegments))
                        continue;

                    return new ResolvedUrl(httpService.Value, method.method, currentSegments, targetSegments);
                }
            }

            return null;
        }

        private static bool TryResolveUrlForMethod(string[] currentSegments, UrlSegment[] targetSegments)
        {
            if (targetSegments.Length != currentSegments.Length)
                return false;

            if (!CompareUrls(currentSegments, targetSegments))
                return false;

            return true;
        }

        private static object[] MapParameters(MethodInfo method, string[] currentSegments, UrlSegment[] targetSegments, Stream data, Encoding encoding)
        {
            var parameterInfoCollection = method.GetParameters();
            var parameters = new object[parameterInfoCollection.Length];
            var i = -1;

            foreach (var targetSegment in targetSegments)
            {
                i++;

                if (!targetSegment.IsParameter)
                    continue;

                var parameterInfoIndex = Array.FindIndex(parameterInfoCollection,
                    x => x.Name.Equals(targetSegment.Name, StringComparison.OrdinalIgnoreCase));

                if (parameterInfoIndex == -1)
                    continue;

                parameters[parameterInfoIndex] = Convert.ChangeType(currentSegments[i], parameterInfoCollection[parameterInfoIndex].ParameterType);
            }

            i = 0;
            foreach (var parameterInfo in parameterInfoCollection)
            {
                if (parameterInfo.Name != "data")
                    continue;

                var json = Json.Parse(data, encoding);

                parameters[i] = json.Deserialize(parameterInfo.ParameterType, new ObjectSerializerOptions()
                {
                    IgnoreMissingProperties = false
                });

                i++;
            }

            return parameters;
        }

        private static bool CompareUrls(string[] currentSegments, UrlSegment[] targetSegments)
        {
            var segmentsCount = Math.Min(currentSegments.Length, targetSegments.Length);

            if (targetSegments.Length > currentSegments.Length)
                return false;

            for (var i = 0; i < segmentsCount; i++)
            {
                var targetSegment = targetSegments[i];

                if (targetSegment.IsParameter)
                    continue;

                var currentSegment = currentSegments[i];

                if (targetSegment.Name != currentSegment)
                    return false;
            }

            return true;
        }

        private static IEnumerable<UrlSegment> GetTargetSegments(Uri uri)
        {
            return GetSegments(uri)
                .Select(x => new UrlSegment(x));
        }

        private static IEnumerable<string> GetSegments(Uri uri)
        {
            if (!uri.IsAbsoluteUri)
                return uri.OriginalString
                    .Split('/', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.ToLower());
            else
                return uri.Segments
                    .Where(x => x != "/")
                    .Select(x => x.TrimEnd('/').ToLower());
        }
    }
}
