using MinecraftServersControl.Common;
using MinecraftServersControl.Core.Abstractions;
using MinecraftServersControl.Logging;
using MinecraftServersControl.Remote.Server.Schema;
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
            IJson json;
            RemoteWebSocketResponse response;
            try
            {
                json = Json.Parse(e.Data);
                response = json.DeserializeCustom<RemoteWebSocketResponse>();
            }
            catch (Exception ex)
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
                    try
                    {
                        responseGeneric = json.DeserializeCustom(listener.responseType);
                    }
                    catch (Exception ex)
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
                .FirstOrDefault(x => x.Attribute.Code == response.Code)?
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

            try
            {
                responseGeneric = json.DeserializeCustom(methodParameter.ParameterType);
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
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
