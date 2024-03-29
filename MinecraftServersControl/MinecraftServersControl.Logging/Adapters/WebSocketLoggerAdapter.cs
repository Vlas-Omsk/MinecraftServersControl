﻿using System;
using WebSocketSharp;

namespace MinecraftServersControl.Logging.Adapters
{
    public sealed class WebSocketLoggerAdapter
    {
        private Logger _logger;

        public WebSocketLoggerAdapter(Logger logger)
        {
            _logger = logger;
        }

        public void Output(LogData data, string str)
        {
            var msg = data.Message;
            if (!string.IsNullOrEmpty(str))
                msg += " " + str;
            
            switch (data.Level)
            {
                case WebSocketSharp.LogLevel.Trace:
                case WebSocketSharp.LogLevel.Debug:
                case WebSocketSharp.LogLevel.Info:
                    _logger.Log(LogLevel.Info, data.Caller, msg);
                    break;
                case WebSocketSharp.LogLevel.Warn:
                    _logger.Log(LogLevel.Warn, data.Caller, msg);
                    break;
                case WebSocketSharp.LogLevel.Error:
                case WebSocketSharp.LogLevel.Fatal:
                    _logger.Log(LogLevel.Error, data.Caller, msg);
                    break;
            }
        }
    }
}
