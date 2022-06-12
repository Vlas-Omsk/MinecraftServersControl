using MinecraftServersControl.Common;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.DTO;
using MinecraftServersControl.Remote.Schema;
using PinkJson2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MinecraftServersControl.Remote.Server.WebSocketServices
{
    public abstract class WebSocketService : WebSocketBehavior
    {
        internal IApplication Application { get; set; }
        internal Logging.Logger Logger { get; set; }

        private List<(int requestId, Type responseType, Action<object> listener)> _listeners = new List<(int, Type, Action<object>)>();

        protected override void OnOpen()
        {
            Logger.Info("Opened " + GetInfo());
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Logger.Info("Closed " + GetInfo());
        }

        protected override async void OnMessage(MessageEventArgs e)
        {
            if (!e.Data.TryParseJson(out IJson json, out Exception ex))
            {
                Logger.Warn(ex.ToString());
                return;
            }

            if (!json.TryDeserialize(out RemoteWebSocketResponse<RemoteResult> response, out ex))
            {
                Logger.Warn(ex.ToString());
                return;
            }

            Logger.Info($"Response: {response}, Client: {GetInfo()}");

            object responseGeneric;

            if (response.RequestId != -1)
            {
                var listener = _listeners.FirstOrDefault(x => x.requestId == response.RequestId);

                _listeners.Remove(listener);

                if (listener != default)
                {
                    if (!json.TryDeserialize(listener.responseType, out responseGeneric, out ex))
                    {
                        Logger.Warn(ex.ToString());
                        return;
                    }

                    listener.listener.Invoke(responseGeneric);
                }

                return;
            }

            var method = GetType()
                .GetMethodsWithAttribute<WebSocketResponseAttribute>()
                .FirstOrDefault(x => x.Attribute.Code == response.Result.Code)?
                .Method;

            if (method == null)
            {
                Logger.Error($"Method not found, Response: {response}");
                return;
            }

            var methodParameter = method.GetParameters().ElementAtOrDefault(0);

            if (methodParameter == null)
            {
                Logger.Error("(" + method.ToString() + $").Parameters.Length != 1, Method: {method}");
                return;
            }

            if (!json.TryDeserialize(methodParameter.ParameterType, out responseGeneric, out ex))
            {
                Logger.Warn(ex.ToString());
                return;
            }

            try
            {
                var methodResult = method.Invoke(this, new object[] { responseGeneric });

                if (methodResult is Task task)
                    await task.ConfigureAwait(false);
            }
            catch (Exception exx)
            {
                Logger.Error(exx.ToString());
                return;
            }
        }

        public Task<TResponse> GetResponse<TResponse, TRequest>(TRequest request)
            where TRequest : RemoteWebSocketRequest
            where TResponse : RemoteWebSocketResponse
        {
            var taskCompletionSource = new TaskCompletionSource<TResponse>();

            _listeners.Add((
                request.Id, 
                typeof(TResponse), 
                (object response) => 
                    taskCompletionSource.SetResult((TResponse)response)
            ));

            SendRequest(request);

            return taskCompletionSource.Task;
        }

        protected void SendRequest(RemoteWebSocketRequest request)
        {
            Logger.Info($"Request: {request}, Client: {GetInfo()}");

            if (ConnectionState != WebSocketState.Open)
                return;

            try
            {
                Send(request.Serialize().ToString());
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        private string GetInfo()
        {
            return Context.UserEndPoint.ToString();
        }
    }
}
