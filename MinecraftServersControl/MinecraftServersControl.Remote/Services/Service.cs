using MinecraftServersControl.Logging;
using System;

namespace MinecraftServersControl.Remote.Services
{
    public abstract class Service
    {
        protected Logger Logger { get; }

        protected Service(Logger logger)
        {
            Logger = logger;
        }
    }
}
