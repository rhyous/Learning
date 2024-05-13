using Rhyous.CS6210.Hw1.Interfaces;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class PullServer : RegistrationClient, IServer<ZFrame>, IDisposable, IName
    {
        public virtual ZContext Context { get; set; }
        public virtual IPullSocket Socket { get; set; }

        public bool IsStarted { get; set; }

        public string Name { get; internal set; }
        
        public PullServer(string name, string nsEndpoint, ILogger logger) 
            : base(nsEndpoint, new SystemRegistration(name), true, logger)
        {
            Name = name;
        }

        public PullServer(string name, string endpoint, string nsEndpoint, ILogger logger)
            : base(nsEndpoint, new SystemRegistration(name, endpoint), true, logger)
        {
            Name = name;
        }

        public async Task StartAsync(string endpoint, Action<ZFrame> receiveAction)
        {
            await StartAsync(string.IsNullOrWhiteSpace(Name) ? endpoint : Name, endpoint, receiveAction);
        }

        public async Task StartAsync(string name, string endpoint, Action<ZFrame> receiveAction)
        {
            Name = name;
            Context = Context ?? new ZContext();
            Socket = Socket ?? new PullSocketAdapter(new ZSocket(Context, ZSocketType.PULL));
            Socket.Bind(endpoint);
            Logger?.WriteLine(Name);
            Logger?.WriteLine($"Started Pull Server on endpoint {endpoint}");
            while (!_IsDisposed)
            {
                await OpenReceiveAsync(receiveAction);
            }
        }

        async Task IServer<ZFrame>.StartAsync(string endpoint, ZSocketType type, Action<ZFrame> receiveAction)
        {
            await StartAsync(endpoint, receiveAction);
        }

        internal async Task OpenReceiveAsync(Action<ZFrame> receiveAction)
        {
            IsStarted = true;
            await Task.Run(() =>
            {
                using (ZFrame request = Socket.ReceiveFrame())
                {
                    receiveAction?.Invoke(request);
                }
            });
        }

        public virtual void Stop()
        {
            Dispose();
        }

        #region IDisposable

        internal bool _IsDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                _IsDisposed = true;
                if (disposing)
                {
                    Socket?.Dispose();
                    Context?.Dispose();
                }
            }
        }

        #endregion
    }
}