using MinecraftServersControl.API.Schema;
using MinecraftServersControl.API.Services;
using MinecraftServersControl.Core.DTO;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API.IntegrationTests.Helpers
{
    public sealed class ApiClient<T> : IClient where T : ApiService, new()
    {
        public bool Closed { get; private set; }

        private readonly T _apiService;
        private int _counter;
        private Action<Response> _listeners;

        public ApiClient(ApiContextFactory apiContextFactory)
        {
            _apiService = apiContextFactory.CreateApiService<T>(this);
        }

        public Task<Response> GetResponse(RequestCode code, object data)
        {
            var id = _counter++;
            var taskCompletionSource = new TaskCompletionSource<Response>();
            Action<Response> handler = null;

            handler = (response) =>
            {
                if (response.RequestId == id)
                {
                    taskCompletionSource.SetResult(response);
                    _listeners -= handler;
                }
            };

            _listeners += handler;

            _apiService.ProcessAsync(new Request(id, code, data)).ConfigureAwait(false);

            return taskCompletionSource.Task;
        }

        public Task<Result<TData>> GetResult<TData>(RequestCode code, object data)
        {
            var id = _counter++;
            var taskCompletionSource = new TaskCompletionSource<Result<TData>>();
            Action<Response> handler = null;

            handler = (response) =>
            {
                if (response.RequestId == id)
                {
                    if (response.Code == ResponseCode.Success)
                        taskCompletionSource.SetResult((Result<TData>)response.Result);
                    else
                        taskCompletionSource.SetException(new ApiException(response));
                    _listeners -= handler;
                }
            };

            _listeners += handler;

            _apiService.ProcessAsync(new Request(id, code, data)).ConfigureAwait(false);

            return taskCompletionSource.Task;
        }

        public Task<Result> GetResult(RequestCode code, object data)
        {
            var id = _counter++;
            var taskCompletionSource = new TaskCompletionSource<Result>();
            Action<Response> handler = null;

            handler = (response) =>
            {
                if (response.RequestId == id)
                {
                    if (response.Code == ResponseCode.Success)
                        taskCompletionSource.SetResult(response.Result);
                    else
                        taskCompletionSource.SetException(new ApiException(response));
                    _listeners -= handler;
                }
            };

            _listeners += handler;

            _apiService.ProcessAsync(new Request(id, code, data)).ConfigureAwait(false);

            return taskCompletionSource.Task;
        }

        public Task<Result<TData>> GetBroadcastResult<TData>(ResponseCode code)
        {
            var taskCompletionSource = new TaskCompletionSource<Result<TData>>();
            Action<Response> handler = null;

            handler = (response) =>
            {
                if (response.RequestId == Response.BroadcastRequestId &&
                    response.Code == code)
                {
                    if (response.Code == ResponseCode.Success)
                        taskCompletionSource.SetResult((Result<TData>)response.Result);
                    else
                        taskCompletionSource.SetException(new ApiException(response));
                    _listeners -= handler;
                }
            };

            _listeners += handler;

            return taskCompletionSource.Task;
        }

        Task IClient.SendResponseAsync(Response response)
        {
            _listeners?.Invoke(response);
            return Task.CompletedTask;
        }

        Task IClient.CloseAsync()
        {
            Closed = true;

            return Task.CompletedTask;
        }
    }
}
