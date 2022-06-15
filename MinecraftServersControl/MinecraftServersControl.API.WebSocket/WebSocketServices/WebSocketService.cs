using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Common;
using MinecraftServersControl.Core.Interface;
using MinecraftServersControl.Logging;
using PinkJson2;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MinecraftServersControl.API.WebSocketServices
{
    public abstract class WebSocketService : WebSocketBehavior
    {
        internal IApplication Application { get; set; }
        internal Logging.Logger Logger { get; set; }

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
            try
            {
                json = Json.Parse(e.Data);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
                SendError(WebSocketResponse.BroadcastRequestId, WebSocketResponseCode.DataError, ex.Message);
                return;
            }

            WebSocketRequest request;
            try
            {
                request = json.DeserializeCustom<WebSocketRequest>();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
                SendError(WebSocketResponse.BroadcastRequestId, WebSocketResponseCode.DataError, ex.Message);
                return;
            }

            Logger.Info($"Request: {request}, Client: {GetInfo()}");

            var method = GetType()
                .GetMethodsWithAttribute<WebSocketRequestAttribute>()
                .FirstOrDefault(x => x.Attribute.Code == request.Code)?
                .Method;

            if (method == null)
            {
                SendError(request.Id, WebSocketResponseCode.InvalidCode, null);
                return;
            }

            var methodParameter = method.GetParameters().ElementAtOrDefault(0);

            if (methodParameter == null)
            {
                Logger.Error("(" + method.ToString() + $").Parameters.Length != 1, Method: {method}");
                SendError(request.Id, WebSocketResponseCode.InternalServerError, null);
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
                SendError(request.Id, WebSocketResponseCode.DataError, ex.Message);
                return;
            }

            try
            {
                var methodResult = method.Invoke(this, new object[] { requestGeneric });

                if (methodResult is Task task)
                    await task.ConfigureAwait(false);
            }
            catch (CoreException ex)
            {
                SendResponse(new WebSocketResponse(request.Id, WebSocketResponseCode.CoreError, ex.ErrorCode, ex.ErrorMessage));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                SendError(request.Id, WebSocketResponseCode.InternalServerError, null);
                return;
            }
        }

        protected void SendError(int requestId, WebSocketResponseCode code, string message)
        {
            SendResponse(new WebSocketResponse(requestId, code, message));
        }

        protected void SendSuccess<T>(int requestId, WebSocketResponseCode code, T data)
        {
            SendResponse(new WebSocketResponse<T>(requestId, code, null, data));
        }

        protected void SendSuccess(int requestId, WebSocketResponseCode code)
        {
            SendResponse(new WebSocketResponse(requestId, code, null));
        }

        private void SendResponse(WebSocketResponse response)
        {
            Logger.Info($"Response: {response}, Client: {GetInfo()}");

            if (ConnectionState != WebSocketState.Open)
                return;

            try
            {
                Send(response.Serialize().ToString());
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
