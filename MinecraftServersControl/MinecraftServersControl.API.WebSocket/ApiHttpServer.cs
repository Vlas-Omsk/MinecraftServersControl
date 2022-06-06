using MinecraftServersControl.API.Schema;
using MinecraftServersControl.API.WebSocket.HttpServices;
using MinecraftServersControl.Core;
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
using WebSocketSharp.Server;

namespace MinecraftServersControl.API.WebSocket
{
    public sealed class ApiHttpServer : HttpServer
    {
        private readonly Application _application;
        private readonly ILogger _logger;
        private readonly Dictionary<Uri, Type> _httpServices = new Dictionary<Uri, Type>();

        public ApiHttpServer(Application application, ILogger logger, string url) : base(url)
        {
            _application = application;
            _logger = logger;

            OnRequest += OnRequestInternal;
        }

        private void OnRequestInternal(object sender, HttpRequestEventArgs e)
        {
            if (!Enum.TryParse(e.Request.HttpMethod, true, out HttpMethod httpMethod))
            {
                e.Response.SendError(HttpStatusCode.MethodNotAllowed);
                return;
            }

            if (httpMethod == HttpMethod.Post && e.Request.ContentType != ContentTypeNames.ApplicationJson)
            {
                e.Response.SendError(HttpStatusCode.BadRequest);
                return;
            }
                
            var result = TryResolveUrl(httpMethod, e.Request.Url);

            if (result == null)
            {
                e.Response.SendError(HttpStatusCode.BadRequest);
                return;
            }

            _logger.Info($"Request: {result.ServiceType.Name}.{result.Method.Name}, HttpMethod: {httpMethod}, Url: {e.Request.Url}, Client: {e.Request.RemoteEndPoint}");

            var parameters = MapParameters(result.Method, result.CurrentSegments, result.TargetSegments, e.Request.InputStream, e.Request.ContentEncoding);

            var objCctor = result.ServiceType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.DefaultBinder, Type.EmptyTypes, null);
            var obj = (HttpService)objCctor.Invoke(null);

            try
            {
                obj.Init(_application, _logger, e.Request, e.Response);

                var methodResult = result.Method.Invoke(obj, parameters);

                if (methodResult is Task task)
                    task.ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                e.Response.SendError(HttpStatusCode.InternalServerError, new HttpErrorResponse(ex.Message));
                return;
            }

            try
            {
                e.Response.SendError(HttpStatusCode.NotImplemented);
            }
            catch
            {
            }
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

        private bool TryResolveUrlForMethod(string[] currentSegments, Segment[] targetSegments)
        {
            if (targetSegments.Length != currentSegments.Length)
                return false;

            if (!CompareUrls(currentSegments, targetSegments))
                return false;

            return true;
        }

        private object[] MapParameters(MethodInfo method, string[] currentSegments, Segment[] targetSegments, Stream data, Encoding encoding)
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

        private static bool CompareUrls(string[] currentSegments, Segment[] targetSegments)
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

        private IEnumerable<Segment> GetTargetSegments(Uri uri)
        {
            return GetSegments(uri)
                .Select(x => new Segment(x));
        }

        private IEnumerable<string> GetSegments(Uri uri)
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

        public void AddHttpService<T>(string path) where T : HttpService, new()
        {
            _httpServices.Add(new Uri(path, UriKind.RelativeOrAbsolute), typeof(T));
        }
    }

    internal class ResolvedUrl
    {
        public Type ServiceType { get; }
        public MethodInfo Method { get; }
        public string[] CurrentSegments { get; }
        public Segment[] TargetSegments { get; }

        public ResolvedUrl(Type serviceType, MethodInfo method, string[] currentSegments, Segment[] targetSegments)
        {
            ServiceType = serviceType;
            Method = method;
            CurrentSegments = currentSegments;
            TargetSegments = targetSegments;
        }
    }

    internal class Segment
    {
        public string Name { get; }
        public bool IsParameter { get; }

        public Segment(string segment)
        {
            IsParameter = segment.StartsWith(':');
            Name = !IsParameter ?
                segment :
                segment.Substring(1, segment.Length - 1);
        }
    }
}
