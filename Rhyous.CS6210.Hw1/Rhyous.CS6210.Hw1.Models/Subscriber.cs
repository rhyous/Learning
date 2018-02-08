using Rhyous.CS6210.Hw1.Interfaces;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class Subsbriber : ISubscribe, IConnect, IDisposable
    {
        public ZContext Context { get; set; }
        public ISubscribeSocket Socket { get; set; }

        public virtual void Connect(string endpoint)
        {
            Context = Context ?? new ZContext();
            Socket = Socket ?? new SubscribeSocketAdapter(new ZSocket(Context, ZSocketType.SUB));
            Socket.Connect(endpoint);
        }

        public bool IsConnected { get { return Socket.IsConnected; } }

        public virtual void Subscribe(string name)
        {
            if (!IsConnected)
                throw new Exception("You must first connnect.");
            Socket.Subscribe(name);           
        }

        #region IDisposable

        private bool _IsDisposed;

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
                    Socket.Dispose();
                    Context.Dispose();
                }
            }
        }

        #endregion
    }
}