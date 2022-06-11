using System;
using System.Diagnostics;

namespace MinecraftServersControl.Logging
{
    public abstract class Logger
    {
        public void Info(string message)
        {
            LogInternal(LogLevel.Info, message);
        }

        public void Warn(string message)
        {
            LogInternal(LogLevel.Warn, message);
        }

        public void Error(string message)
        {
            LogInternal(LogLevel.Error, message);
        }

        private void LogInternal(LogLevel level, string message)
        {
            Log(level, new StackFrame(2, true), message);
        }

        public abstract void Log(LogLevel level, StackFrame frame, string message);
    }
}
