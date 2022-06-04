using System;

namespace MinecraftServersControl.Logging
{
    public sealed class CallbackLogger : ILogger
    {
        private Action<string> _callback;

        public CallbackLogger(Action<string> callback)
        {
            _callback = callback;
        }

        public void Info(string message)
        {
            Write("info", message);
        }

        public void Warn(string message)
        {
            Write("warn", message);
        }

        public void Error(string message)
        {
            Write("error", message);
        }

        private void Write(string header, string message)
        {
            header = $"[{header.ToUpper()}] ";
            _callback(
                header + 
                message.Replace(Environment.NewLine, Environment.NewLine + new string(' ', header.Length))
            );
        }
    }
}
