using System;
using System.Linq;
using System.Reflection;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public class Request
    {
        public RequestCode Code { get; set; }
        public object Data { get; set; }

        public Request()
        {
        }

        public Request(RequestCode code, object data)
        {
            Code = code;
            Data = data;
        }

        public T GetData<T>() => (T)Data;

        public static Type GetDataType(RequestCode code) =>
            typeof(RequestCode)
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .First(x => (RequestCode)x.GetValue(null) == code)
                .GetCustomAttribute<DataTypeAttribute>(false)
                .Type;
    }
}
