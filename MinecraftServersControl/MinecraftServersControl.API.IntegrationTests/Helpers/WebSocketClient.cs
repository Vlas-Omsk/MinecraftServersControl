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
        private MessageReceived _listeners;

        private delegate void MessageReceived(WebSocketResponse response, IJson json);

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
            var json = Json.Parse(e.Data);
            var response = Deserialize<WebSocketResponse>(json);

            _listeners.Invoke(response, json);
        }

        private static T Deserialize<T>(IJson json)
        {
            return json.Deserialize<T>(new ObjectSerializerOptions()
            {
                IgnoreMissingProperties = false
            });
        }

        public Task<WebSocketResponse<TResult>> GetResponse<TResult>(WebSocketRequestCode code)
            where TResult : Result
        {
            return GetResponse<WebSocketRequest, WebSocketResponse<TResult>>(new WebSocketRequest(_counter++, code));
        }

        public Task<WebSocketResponse<Result>> GetResponse<TRequestData>(WebSocketRequestCode code, TRequestData data)
        {
            return GetResponse<WebSocketRequest<TRequestData>, WebSocketResponse<Result>>(new WebSocketRequest<TRequestData>(_counter++, code, data));
        }

        public Task<WebSocketResponse<TResult>> GetResponse<TRequestData, TResult>(WebSocketRequestCode code, TRequestData data)
            where TResult : Result
        {
            return GetResponse<WebSocketRequest<TRequestData>, WebSocketResponse<TResult>>(new WebSocketRequest<TRequestData>(_counter++, code, data));
        }

        private Task<TResponse> GetResponse<TRequest, TResponse>(TRequest request)
            where TRequest : WebSocketRequest
            where TResponse : WebSocketResponse
        {
            var taskCompletionSource = new TaskCompletionSource<TResponse>();
            MessageReceived handler = null;

            handler = (WebSocketResponse response, IJson json) =>
            {
                if (response.RequestId == request.Id)
                {
                    try
                    {
                        taskCompletionSource.TrySetResult(json.Deserialize<TResponse>());
                    }
                    catch (Exception ex)
                    {
                        taskCompletionSource.TrySetException(ex);
                    }
                    finally
                    {
                        _listeners -= handler;
                    }
                }
            };

            _listeners += handler;

            SendRequestAsync(request);

            Task.Delay(5000)
                .ContinueWith(x => taskCompletionSource.TrySetException(new Exception("Timeout")))
                .ConfigureAwait(false);

            return taskCompletionSource.Task;
        }

        public Task<WebSocketResponse<TResult>> GetBroadcastResult<TResult>(ResultCode code)
            where TResult : Result
        {
            var taskCompletionSource = new TaskCompletionSource<WebSocketResponse<TResult>>();
            MessageReceived handler = null;

            handler = (WebSocketResponse response, IJson json) =>
            {
                if (response.RequestId == -1)
                {
                    try
                    {
                        var responseGeneric = json.Deserialize<WebSocketResponse<TResult>>();

                        if (responseGeneric.Result.Code == code)
                        {
                            taskCompletionSource.SetResult(responseGeneric);
                            _listeners -= handler;
                        }
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

            return taskCompletionSource.Task;
        }

        private void SendRequestAsync(WebSocketRequest webSocketRequest)
        {
            var json = webSocketRequest.Serialize();
            _webSocket.Send(json.ToString());
        }
    }
}
