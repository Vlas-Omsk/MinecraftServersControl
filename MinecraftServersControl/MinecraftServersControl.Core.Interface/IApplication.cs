using MinecraftServersControl.Core.Abstractions.Services;
using System;

namespace MinecraftServersControl.Core.Abstractions
{
    public interface IApplication
    {
        IUserService UserService { get; }
        IVkUserService VkUserService { get; }
        IServerService ServerService { get; }
        IComputerService ComputerService { get; }
    }
}
