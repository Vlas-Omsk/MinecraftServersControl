using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;

namespace MinecraftServersControl.Core.IO
{
    public sealed class ProcessHost
    {
        private Process _process;
        private System.Timers.Timer _checkForDataTimer;
        private string _readBuffer = string.Empty;
        private bool _readBufferChanged;
        private Thread _readDataThread;
        private CancellationTokenSource _readDataCancellationTokenSource;
        private Queue<char> _buffer = new Queue<char>();

        private const int _bufferMaxLength = 100;
        private const int _timerInterval = 100;

        public ProcessHost()
        {
        }

        public IEnumerable<char> Buffer => _buffer;

        public void Start(string command)
        {
            if (_process != null)
                throw new InvalidOperationException("The process has already started");

            _process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = command
                }
            };

            _process.Start();

            _checkForDataTimer = new System.Timers.Timer(_timerInterval);
            _checkForDataTimer.Elapsed += OnTimerElapsed;
            _checkForDataTimer.Start();

            _readDataCancellationTokenSource = new CancellationTokenSource();

            _readDataThread = new Thread(ReadDataHandler);
            _readDataThread.Start(_readDataCancellationTokenSource.Token);
        }

        public void Stop()
        {
            _readDataCancellationTokenSource.Cancel();
            _process.Close();
            _process.WaitForExit();
            _readDataThread.Join();
            _readDataCancellationTokenSource.Dispose();
        }

        public void Write(string str)
        {
            _process.StandardInput.WriteLine(str);
        }

        private void WriteBuffer(string str)
        {
            foreach (var ch in str)
                _buffer.Enqueue(ch);

            while (_buffer.Count > _bufferMaxLength - 1)
                _buffer.Dequeue();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            string line;

            lock (_readBuffer)
            {
                if (!_readBufferChanged)
                    return;

                line = _readBuffer;
                _readBuffer = string.Empty;

                WriteBuffer(line);
                RaiseDataReceived(line);

                _readBufferChanged = false;
            }
        }

        private void ReadDataHandler(object obj)
        {
            var cancellationToken = (CancellationToken)obj;

            while (!_process.HasExited && !cancellationToken.IsCancellationRequested)
            {
                var ch = _process.StandardOutput.Read();

                if (ch != -1)
                {
                    lock (_readBuffer)
                    {
                        _readBuffer += (char)ch;
                        _readBufferChanged = true;
                    }
                }
            }
        }

        private void RaiseDataReceived(string data)
        {
            DataReceived?.Invoke(this, new DataReceivedEventArgs(data));
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;
    }
}
