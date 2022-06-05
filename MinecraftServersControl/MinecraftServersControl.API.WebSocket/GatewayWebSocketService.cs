using MinecraftServersControl.API.Schema;
using MinecraftServersControl.API.Services;
using MinecraftServersControl.Logging;
using PinkJson2;
using PinkJson2.Serializers;
using System;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MinecraftServersControl.API.WebSocket
{
    public sealed class GatewayWebSocketService : WebSocketBehavior, IClient
    {
        internal RealtimeApiService ApiService { get; set; }
        internal ILogger Logger { get; set; }

        protected override async void OnOpen()
        {
            await ApiService.OpenAsync();
        }

        protected override async void OnClose(CloseEventArgs e)
        {
            await ApiService.CloseAsync();
        }

        protected sealed override async void OnMessage(MessageEventArgs e)
        {
            WebSocketRequest request = null;
            bool handled = true;

            await Task.Run(() =>
            {
                IJson json;

                try
                {
                    json = Json.Parse(e.Data);
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString());
                    SendResponse(new WebSocketResponse(WebSocketResponse.BroadcastRequestId, ResponseCode.DataError, ex.Message, null));
                    handled = false;
                    return;
                }

                if (!TryDeserializeData(WebSocketResponse.BroadcastRequestId, json, typeof(WebSocketRequest), out object requestObj))
                {
                    handled = false;
                    return;
                }

                request = (WebSocketRequest)requestObj;

                var dataType = Request.GetDataType(request.Code);

                if (dataType != null)
                {
                    if (!TryDeserializeData(request.Id, (IJson)request.Data, dataType, out object dataObject))
                    {
                        handled = false;
                        return;
                    }

                    request.Data = dataObject;
                }
                else
                {
                    request.Data = null;
                }
            });

            if (!handled)
                return;

            await ApiService.ProcessAsync(request);
        }

        private void SendResponse(WebSocketResponse response)
        {
            if (ConnectionState != WebSocketState.Open &&
                ConnectionState != WebSocketState.Connecting)
                return;
            var json = response.Serialize();
            Send(json.ToString());
        }

        string IClient.GetInfo()
        {
            return Context.UserEndPoint.ToString();
        }

        Task IClient.SendResponseAsync(Request request, Response response)
        {
            return Task.Run(() => 
                SendResponse(new WebSocketResponse(((WebSocketRequest)request).Id, response))
            );
        }

        Task IClient.CloseAsync()
        {
            return Task.Run(() => 
                Close()
            );
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
                SendResponse(new WebSocketResponse(requestId, ResponseCode.DataError, ex.Message, null));
                obj = null;
                return false;
            }
        }
    }
}