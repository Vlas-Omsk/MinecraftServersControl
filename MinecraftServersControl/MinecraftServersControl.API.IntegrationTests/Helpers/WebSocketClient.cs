using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core.DTO;
using PinkJson2;
using PinkJson2.Serializers;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace MinecraftServersControl.API.IntegrationTests.Helpers
{
    public sealed class WebSocketClient
    {
        private int _counter;
        private WebSocket _webSocket;
        private Action<WebSocketResponseBase> _listeners;

        public WebSocketClient(string url)
        {
            _webSocket = new WebSocket(url);
            _webSocket.OnMessage += OnWebSocketMessage;
            _webSocket.Connect();
        }

        public bool Closed => _webSocket.ReadyState == WebSocketState.Closed ||
            _webSocket.ReadyState == WebSocketState.Closing;

        private void OnWebSocketMessage(object sender, MessageEventArgs e)
        {
            var responseBase = Json.Parse(e.Data).Deserialize<WebSocketResponseBase>();

            var json = Json.Parse(e.Data);
            var response = json.Deserialize<WebSocketResponseBase>(new ObjectSerializerOptions()
            {
                IgnoreMissingProperties = false
            });

            _listeners.Invoke(response);
        }

        public Task<WebSocketResponse<TResponseData>> GetResponse<TRequestData, TResponseData>(WebSocketRequestCode code, TRequestData data)
        {
            var id = _counter++;
            var taskCompletionSource = new TaskCompletionSource<WebSocketResponse<TResponseData>>();
            Action<WebSocketResponseBase> handler = null;

            handler = (responseBase) =>
            {
                if (responseBase.RequestId == id)
                {
                    try
                    {
                        taskCompletionSource.SetResult(
                            new WebSocketResponse<TResponseData>(
                                responseBase.RequestId,
                                responseBase.Code,
                                responseBase.ErrorMessage,
                                responseBase.Result.Deserialize<Result<TResponseData>>()
                            ));
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.SetException(ex);
                    }
                    finally
                    {
                        _listeners -= handler;
                    }
                }
            };

            _listeners += handler;

            SendRequestAsync(new WebSocketRequest<TRequestData>(id, code, data));

            return taskCompletionSource.Task;
        }

        public Task<WebSocketResponse<TResponseData>> GetBroadcastResult<TResponseData>(ResultCode code)
        {
            var taskCompletionSource = new TaskCompletionSource<WebSocketResponse<TResponseData>>();
            Action<WebSocketResponseBase> handler = null;

            handler = (responseBase) =>
            {
                if (responseBase.RequestId == WebSocketResponse.BroadcastRequestId &&
                    responseBase.Code == WebSocketResponseCode.Success)
                {
                    var response = 
                        new WebSocketResponse<TResponseData>(
                            responseBase.RequestId,
                            responseBase.Code,
                            responseBase.ErrorMessage,
                            responseBase.Result.Deserialize<Result<TResponseData>>()
                        );

                    if (response.Result.Code == code)
                        taskCompletionSource.SetResult(response);
                    _listeners -= handler;
                }
            };

            _listeners += handler;

            return taskCompletionSource.Task;
        }

        public void SendRequestAsync(IWebSocketRequest webSocketRequest)
        {
            var json = webSocketRequest.Serialize();
            _webSocket.Send(json.ToString());
        }
    }
}
