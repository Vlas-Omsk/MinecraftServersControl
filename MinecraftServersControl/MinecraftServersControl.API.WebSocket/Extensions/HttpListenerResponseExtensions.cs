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

        public static void SendSuccess<T>(this HttpListenerResponse self, Result<T> data)
        {
            Send(self, HttpStatusCode.OK, new HttpResponse<T>(data));
        }

        public static void SendError(this HttpListenerResponse self, HttpStatusCode statusCode)
        {
            self.StatusCode = (int)statusCode;
            self.Close();
        }

        public static void SendError(this HttpListenerResponse self, HttpStatusCode statusCode, string errorMessage)
        {
            Send(self, statusCode, new HttpResponse<object>(errorMessage));
        }

        public static void Send<T>(this HttpListenerResponse self, HttpStatusCode statusCode, HttpResponse<T> response)
        {
            var content = Encoding.UTF8.GetBytes(response.Serialize().ToString());

            self.StatusCode = (int)statusCode;
            self.ContentType = ContentTypeNames.ApplicationJson;
            self.ContentEncoding = Encoding.UTF8;
            self.ContentLength64 = content.Length;
            self.Close(content, true);
        }
    }
}
