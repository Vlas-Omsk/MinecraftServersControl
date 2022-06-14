using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Timers;

namespace MinecraftServersControl.Remote
{
    public sealed class ProcessHost
    {
        public bool Running { get; private set; }

        private Process _process;
        private System.Timers.Timer _checkForDataTimer;
        private string _readBuffer = string.Empty;
        private bool _readBufferChanged;
        private Thread _readDataThread;
        private CancellationTokenSource _readDataCancellationTokenSource;
        private Queue<char> _buffer = new Queue<char>();
        private bool _stopIsExpected = false;

        private const int _bufferMaxLength = 500;
        private const int _timerInterval = 100;

        public ProcessHost()
        {
        }

        public IEnumerable<char> Buffer => _buffer;

        public void Start(string command)
        {
            if (Running)
                throw new InvalidOperationException("The process has already started");

            Running = true;

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

            try
            {
                _process.Start();
            }
            catch
            {
                Running = false;
                throw;
            }

            _checkForDataTimer = new System.Timers.Timer(_timerInterval);
            _checkForDataTimer.Elapsed += OnTimerElapsed;
            _checkForDataTimer.Start();

            _readDataCancellationTokenSource = new CancellationTokenSource();

            _readDataThread = new Thread(ReadDataHandler);
            _readDataThread.Start(_readDataCancellationTokenSource.Token);

            RaiseStarted();
        }

        public void Stop()
        {
            if (!Running)
                throw new InvalidOperationException("The process has already stopped");

            _stopIsExpected = true;

            OnStop();

            _stopIsExpected = false;
        }

        private void OnStop()
        {
            Running = false;

            _readDataCancellationTokenSource.Cancel();
            _readDataThread.Join();
            _process.Close();
            _readDataCancellationTokenSource.Dispose();

            RaiseStopped();
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

        private async void ReadDataHandler(object obj)
        {
            var cancellationToken = (CancellationToken)obj;
            var buffer = new char[1];

            while (!cancellationToken.IsCancellationRequested)
            {
                var count = await _process.StandardOutput.ReadAsync(buffer, cancellationToken);

                if (count > 0)
                {
                    lock (_readBuffer)
                    {
                        _readBuffer += buffer[0];
                        _readBufferChanged = true;
                    }
                }
            }

            if (!_stopIsExpected)
                OnStop();
        }

        private void RaiseDataReceived(string data)
        {
            DataReceived?.Invoke(this, new DataReceivedEventArgs(data));
        }

        private void RaiseStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseStopped()
        {
            Stopped?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler<DataReceivedEventArgs> DataReceived;
        public event EventHandler Started;
        public event EventHandler Stopped;
    }
}
