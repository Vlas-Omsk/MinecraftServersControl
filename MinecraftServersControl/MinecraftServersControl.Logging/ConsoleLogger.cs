using System;

namespace MinecraftServersControl.Logging
{
    public sealed class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            Write(null, "info", message);
        }

        public void Warn(string message)
        {
            Write(ConsoleColor.Yellow, "warn", message);
        }

        public void Error(string message)
        {
            Write(ConsoleColor.Red, "error", message);
        }

        private static void Write(ConsoleColor? color, string header, string message)
        {
            if (color.HasValue)
                Console.ForegroundColor = color.Value;
            Console.Write(header = $"[{header.ToUpper()}] ");
            if (color.HasValue)
                Console.ResetColor();
            Console.WriteLine(message
                .Replace(Environment.NewLine, Environment.NewLine + new string(' ', header.Length)));
        }
    }
}
