using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MinecraftServersControl.Logging
{
    public sealed class CompositeLogger : Logger
    {
        private List<Logger> _loggers = new List<Logger>();

        public override void Log(LogLevel level, StackFrame frame, string message)
        {
            foreach (var logger in _loggers)
                logger.Log(level, frame, message);
        }

        public void AddLogger(Logger logger)
        {
            _loggers.Add(logger);
        }

        public void RemoveLogger(Logger logger)
        {
            _loggers.Remove(logger);
        }
    }
}
