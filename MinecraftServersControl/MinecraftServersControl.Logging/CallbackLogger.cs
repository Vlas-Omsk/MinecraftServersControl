using System;
using System.Diagnostics;

namespace MinecraftServersControl.Logging
{
    public sealed class CallbackLogger : Logger
    {
        private Action<string> _callback;

        public CallbackLogger(Action<string> callback)
        {
            _callback = callback;
        }

        public override void Log(LogLevel level, StackFrame frame, string message)
        {
            var header = $"[{level.ToString().ToUpper()}] ";
            _callback(
                header +
                message.Replace(Environment.NewLine, Environment.NewLine + new string(' ', header.Length))
            );
        }
    }
}
