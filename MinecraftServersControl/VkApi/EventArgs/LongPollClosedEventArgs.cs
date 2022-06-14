using System;

namespace VkApi
{
    public sealed class LongPollClosedEventArgs : EventArgs
    {
        public Exception Exception { get; }

        public LongPollClosedEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
