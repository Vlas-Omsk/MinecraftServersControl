using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core;
using MinecraftServersControl.Core.DTO;
using MinecraftServersControl.Logging;
using PinkJson2;
using PinkJson2.Serializers;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MinecraftServersControl.API.WebSocketServices
{
    public abstract class WebSocketService : WebSocketBehavior
    {
        internal ILogger Logger { get; set; }
        internal Application Application { get; set; }

        protected override void OnOpen()
        {
            Logger.Info("Opened " + GetInfo());
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Logger.Info("Closed " + GetInfo());
        }

        protected sealed override async void OnMessage(MessageEventArgs e)
        {
            await Task.Run(() => ProcessMessage(e.Data));
        }

        private void ProcessMessage(string data)
        {
            IJson json;

            try
            {
                json = Json.Parse(data);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
                SendError(WebSocketResponse.BroadcastRequestId, WebSocketResponseCode.DataError, ex.Message);
                return;
            }

            if (!TryDeserializeData(WebSocketResponse.BroadcastRequestId, json, typeof(WebSocketRequestBase), out object requestBaseObj))
                return;

            var requestBase = (WebSocketRequestBase)requestBaseObj;

            Logger.Info($"Request: {requestBase.Code}, Client: {GetInfo()}");

            var method = GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(x =>
                {
                    var webSocketRequestAttribute = x.GetCustomAttribute<WebSocketRequestAttribute>();
                    return webSocketRequestAttribute != null && webSocketRequestAttribute.RequestCode == requestBase.Code;
                });

            if (method == null)
            {
                SendError(requestBase.Id, WebSocketResponseCode.InvalidCode, null);
                return;
            }

            var requestType = method.GetParameters()[0].ParameterType;
            var dataType = requestType.GetGenericArguments()[0];
            object dataObject = null;

            if (!TryDeserializeData(requestBase.Id, requestBase.Data, dataType, out dataObject))
                return;

            var requestCctor = requestType
                .GetConstructor(new Type[] { typeof(int), typeof(WebSocketRequestCode), dataType });

            var request = (IWebSocketRequest)requestCctor.Invoke(new object[] { requestBase.Id, requestBase.Code, dataObject });

            try
            {
                var methodResult = method.Invoke(this, new object[] { request });

                if (methodResult is Task task)
                    task.ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                SendError(request.Id, WebSocketResponseCode.InternalServerError, null);
                return;
            }
        }

        private bool TryDeserializeData(int requestId, IJson json, Type type, out object obj)
        {
            try
            {
                obj = json.Deserialize(type, new ObjectSerializerOptions()
                {
                    IgnoreMissingProperties = false
                });
                return true;
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
                SendError(requestId, WebSocketResponseCode.DataError, ex.Message);
                obj = null;
                return false;
            }
        }

        protected void SendError(int requestId, WebSocketResponseCode code, string message)
        {
            SendResponse(new WebSocketResponse<object>(requestId, code, message, null));
        }

        protected void SendSuccess<T>(int requestId, Result<T> result)
        {
            SendResponse(new WebSocketResponse<T>(requestId, WebSocketResponseCode.Success, null, result));
        }

        private void SendResponse(IWebSocketResponse response)
        {
            Logger.Info($"Response: {response.Code}, Result: {response?.Result?.Code}, Client: {GetInfo()}");

            if (ConnectionState != WebSocketState.Open)
                return;

            var json = response.Serialize();
            Send(json.ToString());
        }

        private string GetInfo()
        {
            return Context.UserEndPoint.ToString();
        }
    }
}