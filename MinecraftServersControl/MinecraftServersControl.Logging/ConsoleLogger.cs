using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MinecraftServersControl.Logging
{
    public sealed class ConsoleLogger : Logger
    {
        private int _maxHeaderLength;

        public override void Log(LogLevel level, StackFrame frame, string message)
        {
            ConsoleColor? color = level switch
            {
                LogLevel.Info => null,
                LogLevel.Warn => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                _ => throw new ArgumentOutOfRangeException()
            };

            StackFrame userCodeFrame;
            MethodBase method;

            for (var i = 3; ; i++)
            {
                userCodeFrame = new StackFrame(i, false);
                method = userCodeFrame.GetMethod();
                if (!method.DeclaringType.FullName.StartsWith("System.Runtime.CompilerServices") &&
                    method.DeclaringType.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length == 0)
                    break;
            }

            if (color.HasValue)
                Console.ForegroundColor = color.Value;
            var levelName = level.ToString();
            var position = $"{frame.GetFileLineNumber()}:{frame.GetFileColumnNumber()} ";
            var header = $"[{method.DeclaringType.Name}.{method.Name} ";
            Console.Write(header);
            var headerLength = header.Length + position.Length + levelName.Length + 2;
            if (headerLength > _maxHeaderLength)
                _maxHeaderLength = headerLength;
            else
                Console.Write(new string(' ', _maxHeaderLength - headerLength));
            Console.Write($"{position}{levelName.ToUpper()}] ");
            if (color.HasValue)
                Console.ResetColor();
            Console.WriteLine(message
                .Replace(Environment.NewLine, Environment.NewLine + new string(' ', header.Length)));
        }
    }
}
