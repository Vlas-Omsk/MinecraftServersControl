using MinecraftServersControl.Common;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Logging.Adapters;
using MinecraftServersControl.Remote.Core;
using MinecraftServersControl.Remote.Server.Schema;
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
                OnCloseOverride(sender, e);
            }
        }

        private async void OnMessageInternal(object sender, MessageEventArgs e)
        {
            IJson json;
            RemoteWebSocketRequest request;
            try
            {
                json = Json.Parse(e.Data);
                request = json.DeserializeCustom<RemoteWebSocketRequest>();
            }
            catch (Exception ex)
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

            object requestGeneric;
            try
            {
                requestGeneric = json.DeserializeCustom(methodParameter.ParameterType);
            }
            catch (Exception ex)
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
            catch (RemoteCoreException ex)
            {
                SendResponse(new RemoteWebSocketResponse(request.Id, RemoteWebSocketResponseCode.CoreError, ex.ErrorCode));
                return;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return;
            }
        }

        protected void SendResponse(RemoteWebSocketResponse response)
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
