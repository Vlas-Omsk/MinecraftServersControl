using MinecraftServersControl.API.Schema;
using System;
using System.Threading.Tasks;

namespace MinecraftServersControl.API
{
    public interface IClient
    {
        string GetInfo();
        Task SendResponseAsync(Request targetRequest, Response response);
        Task CloseAsync();
    }
}
