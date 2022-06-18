using MinecraftServersControl.Logging;
using System;

namespace MinecraftServersControl.Remote.Core.Services
{
    public abstract class Service
    {
        protected Logger Logger { get; }

        internal Service(Logger logger)
        {
            Logger = logger;
        }
    }
}
