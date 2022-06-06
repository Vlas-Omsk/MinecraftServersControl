using MinecraftServersControl.API.Schema;
using MinecraftServersControl.Core.DTO;
using PinkJson2;
using System;
using System.Text;
using WebSocketSharp.Net;

namespace MinecraftServersControl.API
{
    internal static class HttpListenerResponseExtensions
    {
        public static void SendSuccess(this HttpListenerResponse self)
        {
            self.StatusCode = (int)HttpStatusCode.OK;
            self.Close();
        }

        public static void SendSuccess(this HttpListenerResponse self, Result data)
        {
            Send(self, HttpStatusCode.OK, data.Serialize());
        }

        public static void SendError(this HttpListenerResponse self, HttpStatusCode statusCode)
        {
            self.StatusCode = (int)statusCode;
            self.Close();
        }

        public static void SendError(this HttpListenerResponse self, HttpStatusCode statusCode, HttpErrorResponse data)
        {
            Send(self, statusCode, data.Serialize());
        }

        public static void Send(this HttpListenerResponse self, HttpStatusCode statusCode, IJson data)
        {
            var content = Encoding.UTF8.GetBytes(data.ToString());

            self.StatusCode = (int)statusCode;
            self.ContentType = ContentTypeNames.ApplicationJson;
            self.ContentEncoding = Encoding.UTF8;
            self.ContentLength64 = content.Length;
            self.Close(content, true);
        }
    }
}
