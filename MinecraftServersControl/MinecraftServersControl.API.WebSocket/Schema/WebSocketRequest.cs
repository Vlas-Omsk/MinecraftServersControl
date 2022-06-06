using System;
using System.Linq;
using System.Reflection;

namespace MinecraftServersControl.API.Schema
{

    [Serializable]
    public class WebSocketRequest
    {
        public int Id { get; set; }
        public WebSocketRequestCode Code { get; set; }
        public object Data { get; set; }

        public WebSocketRequest()
        {
        }

        public WebSocketRequest(int id, WebSocketRequestCode code, object data)
        {
            Id = id;
            Code = code;
            Data = data;
        }

        public T GetData<T>() => (T)Data;

        public static Type GetDataType(WebSocketRequestCode code) =>
            typeof(WebSocketRequestCode)
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .First(x => (WebSocketRequestCode)x.GetValue(null) == code)
                .GetCustomAttribute<DataTypeAttribute>(false)
                .Type;
    }
}
