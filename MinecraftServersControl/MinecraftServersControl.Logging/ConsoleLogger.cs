using System;
using System.Diagnostics;

namespace MinecraftServersControl.Logging
{
    public sealed class ConsoleLogger : Logger
    {
        private int _maxHeaderLength;

        public override void Log(LogLevel level, StackFrame frame, string message)
        {
            var method = frame.GetMethod();
            ConsoleColor? color = level switch
            {
                LogLevel.Info => null,
                LogLevel.Warn => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                _ => throw new ArgumentOutOfRangeException()
            };
            var header = level.ToString();

            if (color.HasValue)
                Console.ForegroundColor = color.Value;
            Console.Write(header = $"[{method.DeclaringType.Name}.{method.Name} {header.ToUpper()}] ");
            if (header.Length > _maxHeaderLength)
                _maxHeaderLength = header.Length;
            else
                Console.Write(new string(' ', _maxHeaderLength - header.Length));
            if (color.HasValue)
                Console.ResetColor();
            Console.WriteLine(message
                .Replace(Environment.NewLine, Environment.NewLine + new string(' ', header.Length)));
        }
    }
}
