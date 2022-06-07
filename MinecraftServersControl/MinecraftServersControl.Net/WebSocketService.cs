using MinecraftServersControl.Logging;
using PinkJson2;
using PinkJson2.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace MinecraftServersControl.Net
{
    public interface IWebSocketRequest<TCode> where TCode : Enum
    {
        int Id { get; }
        TCode Code { get; }
    }

    public interface IWebSocketRequestAttribute<TCode> where TCode : Enum
    {
        TCode Code { get; }
    }

    public abstract class _2WebSocketService<TRequestAttribute, TCode> : WebSocketBehavior
        where TRequestAttribute : Attribute, IWebSocketRequestAttribute<TCode>
        where TCode : Enum
    {
        internal ILogger Logger { get; set; }

        private int _broadcastRequestId = -1;

        protected override void OnOpen()
        {
            Logger.Info("Opened " + GetInfo());
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Logger.Info("Closed " + GetInfo());
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            IJson json;
            int requestId;
            TCode requestCode;

            try
            {
                json = Json.Parse(e.Data);
                requestId = json["Id"].Get<int>();
                requestCode = json["Code"].Get<TCode>();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
                SendDataError(_broadcastRequestId, ex.Message);
                return;
            }

            var codeComparer = EqualityComparer<TCode>.Default;
            var method = GetType()
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(x =>
                {
                    var requestAttribute = x.GetCustomAttribute<TRequestAttribute>();
                    return requestAttribute != null && codeComparer.Equals(requestAttribute.Code, requestCode);
                });
            var methodParameters = method.GetParameters();

            if (methodParameters.Length != 1)
                throw new Exception("(" + method.ToString() + ").Parameters.Length != 1");

            var requestType = methodParameters[0].ParameterType;

            object request;

            try
            {
                request = json.Deserialize(requestType, new ObjectSerializerOptions()
                {
                    IgnoreMissingProperties = false
                });
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString());
                SendDataError(requestId, ex.Message);
                return;
            }

            Logger.Info($"Request: {request}, Client: {GetInfo()}");

            if (method == null)
            {
                SendInvalidCode(requestId, null);
                return;
            }

            try
            {
                var methodResult = method.Invoke(this, new object[] { request });

                if (methodResult is Task task)
                    task.ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                SendInternalServerError(requestId, null);
                return;
            }
        }

        protected abstract void SendDataError(int requestId, string message);
        protected abstract void SendInvalidCode(int requestId, string message);
        protected abstract void SendInternalServerError(int requestId, string message);

        private string GetInfo()
        {
            return Context.UserEndPoint.ToString();
        }
    }
}
