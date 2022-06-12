using MinecraftServersControl.Core.DTO;
using System;

namespace MinecraftServersControl.Core.Interface
{
    public delegate void ResultEventHandler<T>(object sender, Result<T> e);
}
