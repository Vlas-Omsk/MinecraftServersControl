using System;
using System.Linq;
using System.Reflection;

namespace MinecraftServersControl.API.Schema
{
    [Serializable]
    public sealed class Request
    {
        public int Id { get; set; }
        public RequestCode Code { get; set; }
        public object Data { get; set; }

        public Request()
        {
        }

        public Request(int id, RequestCode code, object data)
        {
            Id = id;
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
