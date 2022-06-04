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
    public sealed class WebSocketService : WebSocketBehavior, IClient
    {
        internal ApiService ApiService { get; set; }
        internal ILogger Logger { get; set; }

        protected override async void OnOpen()
        {
            await ApiService.OnConnectedAsync();
        }

        protected sealed override async void OnMessage(MessageEventArgs e)
        {
            Request request = null;

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
                    SendResponse(new Response(Response.BroadcastRequestId, ResponseCode.DataError, ex.Message, null));
                    return;
                }

                request = (Request)TryDeserializeData(Response.BroadcastRequestId, json, typeof(Request));
                if (request == null)
                    return;

                var dataType = Request.GetDataType(request.Code);

                if (dataType != null)
                    request.Data = TryDeserializeData(request.Id, (IJson)request.Data, dataType);
                else
                    request.Data = null;
            });

            if (request == null)
                return;

            await ApiService.ProcessAsync(request);
        }

        protected override async void OnClose(CloseEventArgs e)
        {
            await ApiService.DisposeAsync();
        }

        private void SendResponse(Response response)
        {
            var json = response.Serialize();
            Send(json.ToString());
        }

        string IClient.GetInfo()
        {
            return Context.UserEndPoint.ToString();
        }

        Task IClient.SendResponseAsync(Response response)
        {
            if (ConnectionState != WebSocketState.Open &&
                ConnectionState != WebSocketState.Connecting)
                return Task.CompletedTask;
            return Task.Run(() => SendResponse(response));
        }

        Task IClient.CloseAsync()
        {
            return Task.Run(() => Close());
        }

        private object TryDeserializeData(int requestId, IJson json, Type type)
        {
            try
            {
                return json.Deserialize(type, new ObjectSerializerOptions()
                {
                    IgnoreMissingProperties = false
                });
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
                SendResponse(new Response(requestId, ResponseCode.DataError, ex.Message, null));
                return default;
            }
        }
    }
}