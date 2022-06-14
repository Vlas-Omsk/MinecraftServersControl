using MinecraftServersControl.Core.Interface.Services;
using System;

namespace MinecraftServersControl.Core.Interface
{
    public interface IApplication
    {
        IUserService UserService { get; }
        IVkUserService VkUserService { get; }
        IServerService ServerService { get; }
        IComputerService ComputerService { get; }
    }
}
