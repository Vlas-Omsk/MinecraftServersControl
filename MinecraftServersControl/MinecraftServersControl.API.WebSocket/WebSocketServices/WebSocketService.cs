using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Common;
using MinecraftServersControl.Core;
using MinecraftServersControl.Core.DTO;
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
        internal Application Application { get; set; }
        internal ILogger Logger { get; set; }

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
                SendError(WebSocketResponse.BroadcastRequestId, WebSocketResponseCode.DataError, ex.Message);
                return;
            }

            if (!json.TryDeserialize(out WebSocketRequest request, out ex))
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
                Logger.Error("(" + method.ToString() + ").Parameters.Length != 1");
                SendError(request.Id, WebSocketResponseCode.InternalServerError, null);
                return;
            }

            if (!json.TryDeserialize(methodParameter.ParameterType, out object requestGeneric, out ex))
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
            catch (Exception exx)
            {
                Logger.Error(exx.ToString());
                SendError(request.Id, WebSocketResponseCode.InternalServerError, null);
                return;
            }
        }

        protected void SendError(int requestId, WebSocketResponseCode code, string message)
        {
            SendResponse(new WebSocketResponse(requestId, code, message));
        }

        protected void SendSuccess(int requestId, Result result)
        {
            SendResponse(new WebSocketResponse<Result>(requestId, WebSocketResponseCode.Success, null, result));
        }

        protected void SendSuccess<T>(int requestId, Result<T> result)
        {
            SendResponse(new WebSocketResponse<Result<T>>(requestId, WebSocketResponseCode.Success, null, result));
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
