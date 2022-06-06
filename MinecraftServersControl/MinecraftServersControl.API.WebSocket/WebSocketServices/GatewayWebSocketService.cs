using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core;
using MinecraftServersControl.Logging;
using PinkJson2;
using PinkJson2.Serializers;
using System;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MinecraftServersControl.API.WebSocket
{
    public sealed class GatewayWebSocketService : WebSocketBehavior
    {
        internal Application Application { get; set; }
        internal ILogger Logger { get; set; }

        private AuthState _state;
        private Guid _sessionId;

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
            var request = DeserializeRequest(e.Data);

            if (request == null)
                return;

            await ProcessAsync(request);
        }

        private WebSocketRequest DeserializeRequest(string data)
        {
            IJson json;

            try
            {
                json = Json.Parse(data);
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
                SendResponse(WebSocketResponse.CreateError(WebSocketResponse.BroadcastRequestId, WebSocketResponseCode.DataError, ex.Message));
                return null;
            }

            if (!TryDeserializeData(WebSocketResponse.BroadcastRequestId, json, typeof(WebSocketRequest), out object requestObj))
                return null;

            var request = (WebSocketRequest)requestObj;
            var dataType = WebSocketRequest.GetDataType(request.Code);

            if (dataType != null)
            {
                if (!TryDeserializeData(request.Id, (IJson)request.Data, dataType, out object dataObject))
                    return null;

                request.Data = dataObject;
            }
            else
            {
                request.Data = null;
            }

            return request;
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
                SendResponse(WebSocketResponse.CreateError(requestId, WebSocketResponseCode.DataError, ex.Message));
                obj = null;
                return false;
            }
        }

        private async Task ProcessAsync(WebSocketRequest request)
        {
            Logger.Info($"Request: {request.Code}, Client: {GetInfo()}");

            try
            {
                if (await TryProcessAsync(request))
                    return;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                SendResponse(WebSocketResponse.CreateError(request.Id, WebSocketResponseCode.InternalServerError, null));
                return;
            }

            SendResponse(WebSocketResponse.CreateError(request.Id, WebSocketResponseCode.InvalidCode, null));
        }

        private async Task<bool> TryProcessAsync(WebSocketRequest request)
        {
            switch (request.Code)
            {
                case WebSocketRequestCode.Auth:
                    await Auth(request);
                    return true;
            }

            if (_state == AuthState.Unauthorized)
            {
                SendResponse(WebSocketResponse.CreateError(request.Id, WebSocketResponseCode.InvalidState, null));
                return true;
            }

            switch (request.Code)
            {
                case WebSocketRequestCode.GetServers:
                    await GetServers(request);
                    return true;
            }

            return false;
        }

        private async Task Auth(WebSocketRequest request)
        {
            if (_state != AuthState.Unauthorized)
            {
                SendResponse(WebSocketResponse.CreateError(request.Id, WebSocketResponseCode.InvalidState, null));
                return;
            }

            var sessionId = request.GetData<Guid>();
            var result = await Application.UserService.VerifySession(sessionId);

            SendResponse(WebSocketResponse.CreateSuccess(request.Id, result));

            if (result.HasErrors())
                return;

            _sessionId = sessionId;
            _state = AuthState.Success;
            Application.UserService.SessionRemoved += OnSessionRemoved;
        }

        private async void OnSessionRemoved(object sender, Core.DTO.Result<Guid> e)
        {
            await Task.Run(() =>
            {
                if (_sessionId != e.Data)
                    return;

                SendResponse(WebSocketResponse.CreateSuccess(WebSocketResponse.BroadcastRequestId, e));
                CloseAsync();
            });
        }

        private async Task GetServers(WebSocketRequest request)
        {
            var result = await Application.ServerService.GetServers(_sessionId);

            SendResponse(WebSocketResponse.CreateSuccess(request.Id, result));
        }

        private void SendResponse(WebSocketResponse response)
        {
            if (ConnectionState != WebSocketState.Open &&
                ConnectionState != WebSocketState.Connecting)
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