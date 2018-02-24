using Rhyous.CS6210.Hw1.Interfaces;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class PullServer : RegistrationClient, IServer<ZFrame>, IDisposable, IName
    {
        public virtual ZContext Context { get; set; }
        public virtual IPullSocket Socket { get; set; }
        public string Name { get; internal set; }
        
        public PullServer(string name, string nsEndpoint) : base(nsEndpoint, new SystemRegistration { Name = name })
        {
            Name = name;
        }

        public void Start(string endpoint, Action<ZFrame> receiveAction)
        {
            Start(string.IsNullOrWhiteSpace(Name) ? endpoint : Name, endpoint, receiveAction);
        }

        public void Start(string name, string endpoint, Action<ZFrame> receiveAction)
        {
            Name = name;
            Context = Context ?? new ZContext();
            Socket = Socket ?? new PullSocketAdapter(new ZSocket(Context, ZSocketType.PULL));
            Socket.Bind(endpoint);
            Console.WriteLine(Name);
            Console.WriteLine($"Started Pull Server on endpoint {endpoint}");
            while (!_IsDisposed)
            {
                OpenReceive(receiveAction);
            }
        }

        void IServer<ZFrame>.Start(string endpoint, ZSocketType type, Action<ZFrame> receiveAction)
        {
            Start(endpoint, receiveAction);
        }

        internal void OpenReceive(Action<ZFrame> receiveAction)
        {
            using (ZFrame request = Socket.ReceiveFrame())
            {
                receiveAction?.Invoke(request);
            }
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