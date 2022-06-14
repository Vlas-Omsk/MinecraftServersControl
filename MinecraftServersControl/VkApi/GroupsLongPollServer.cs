using PinkJson2;
using PinkJson2.Serializers;
using System;
using System.Text;
using System.Threading;
using VkApi.Models;
using WebLib;

namespace VkApi
{
    public sealed class GroupsLongPollServer
    {
        private string _key;
        private string _server;
        private string _ts;
        private VkClient _client;
        private int _groupId;
        private Thread _updateThread;
        private bool _running;

        public GroupsLongPollServer(VkClient client, int groupId)
        {
            _client = client;
            _groupId = groupId;
        }

        public void Start()
        {
            if (_running)
                return;

            _running = true;

            var info = GetInfo();
            _key = info.Key;
            _server = info.Server;
            _ts = info.Ts;

            _updateThread = new Thread(UpdateHandler);
            _updateThread.Start();
        }

        public void Stop()
        {
            if (!_running)
                return;

            _running = false;

            _updateThread.Join();
        }

        private void UpdateHandler()
        {
            var httpRequest = HttpRequest.GET;
            httpRequest.Url = _server;
            httpRequest.SetQueryParam("act", "a_check");
            httpRequest.SetQueryParam("wait", 25);

            try
            {
                while (_running)
                {
                    httpRequest.SetQueryParam("key", _key);
                    httpRequest.SetQueryParam("ts", _ts);

                    var httpResponse = httpRequest.GetResponse();
                    using var stream = httpResponse.GetStream();
                    var json = Json.Parse(stream, Encoding.UTF8);
                    var response = json.Deserialize<LongPollResponse>(VkClient.ObjectSerializerOptions);

                    LongPollServerInfo info;

                    switch (response.Failed)
                    {
                        case 1:
                            _ts = response.Ts;
                            break;
                        case 2:
                            info = GetInfo();
                            _key = info.Key;
                            break;
                        case 3:
                            info = GetInfo();
                            _key = info.Key;
                            _ts = info.Ts;
                            break;
                        default:
                            _ts = response.Ts;
                            if (response.Updates.Length > 0)
                                RaiseUpdate(response.Updates);
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                _running = false;
                RaiseClosed(ex);
                throw;
            }

            RaiseClosed(null);
        }

        private LongPollServerInfo GetInfo()
        {
            var request = _client.CreateRequest("groups", "getLongPollServer");
            request.SetParameter("group_id", _groupId);
            return request.GetReponse<VkResponse<LongPollServerInfo>>().ConfigureAwait(false).GetAwaiter().GetResult().Response;
        }

        private void RaiseUpdate(LongPollUpdate[] updates)
        {
            Update?.Invoke(this, new LongPollUpdateEventArgs(updates));
        }

        private void RaiseClosed(Exception exception)
        {
            Closed?.Invoke(this, new LongPollClosedEventArgs(exception));
        }

        public event EventHandler<LongPollUpdateEventArgs> Update;
        public event EventHandler<LongPollClosedEventArgs> Closed;
    }
}
