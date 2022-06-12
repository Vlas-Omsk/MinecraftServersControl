using MinecraftServersControl.Common;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Logging.Adapters;
using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Schema;
using PinkJson2;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebSocketSharp;

namespace MinecraftServersControl.Remote.Client
{
    public abstract class RemoteWebSocketClientBase
    {
        protected Logging.Logger Logger { get; }
        protected Application Application { get; }

        private WebSocket _webSocket;
        private TimeSpan _reconnectDelay;

        public RemoteWebSocketClientBase(string url, TimeSpan reconnectDelay, Logging.Logger logger, Application application)
        {
            _webSocket = new WebSocket(url);
            _webSocket.Log.Output = new WebSocketLoggerAdapter(logger).Output;
            _webSocket.OnOpen += OnOpenOverride + Opened;
            _webSocket.OnClose += OnCloseOverride + Closed;
            _webSocket.OnMessage += OnMessageInternal;

            Logger = logger;
            Application = application;
            _reconnectDelay = reconnectDelay;
        }

        public void Connect()
        {
            _webSocket.Connect();
        }

        protected virtual void OnOpenOverride(object sender, EventArgs e)
        {
            Logger.Info("Opened");
        }

        protected virtual async void OnCloseOverride(object sender, CloseEventArgs e)
        {
            Logger.Info("Closed");

            await Task.Delay(_reconnectDelay);

            try
            {
                Connect();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        private async void OnMessageInternal(object sender, MessageEventArgs e)
        {
            if (!e.Data.TryParseJson(out IJson json, out Exception ex))
            {
                Logger.Error(ex.ToString());
                return;
            }

            if (!json.TryDeserialize(out RemoteWebSocketRequest request, out ex))
            {
                Logger.Error(ex.ToString());
                return;
            }

            Logger.Info($"Request: {request}");

            var method = GetType()
                .GetMethodsWithAttribute<WebSocketRequestAttribute>()
                .FirstOrDefault(x => x.Attribute.Code == request.Code)?
                .Method;

            if (method == null)
            {
                Logger.Error($"Method not found, Request: {request}");
                return;
            }

            var methodParameter = method.GetParameters().ElementAtOrDefault(0);

            if (methodParameter == null)
            {
                Logger.Error("(" + method.ToString() + $").Parameters.Length != 1, Method: {method}");
                return;
            }

            if (!json.TryDeserialize(methodParameter.ParameterType, out object requestGeneric, out ex))
            {
                Logger.Warn(ex.ToString());
                return;
            }

            try
            {
                var methodResult = method.Invoke(this, new object[] { requestGeneric });

                if (methodResult is Task task)
                    await task.ConfigureAwait(false);
            }
            catch (Exception exx)
            {
                Logger.Error(exx.ToString());
                return;
            }
        }

        protected void SendResponse(int requestId, RemoteResult result)
        {
            SendResponse(new RemoteWebSocketResponse<RemoteResult>(requestId, result));
        }

        protected void SendResponse<T>(int requestId, RemoteResult<T> result)
        {
            SendResponse(new RemoteWebSocketResponse<RemoteResult<T>>(requestId, result));
        }

        private void SendResponse(RemoteWebSocketResponse response)
        {
            Logger.Info($"Response: {response}");

            if (_webSocket.ReadyState != WebSocketState.Open)
                return;

            try
            {
                _webSocket.Send(response.Serialize().ToString());
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        public event EventHandler Opened;
        public event EventHandler<CloseEventArgs> Closed;
    }
}
