using Rhyous.CS6210.Hw1.Interfaces;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class SendClient : ISend, IDisposable
    {
        public ZContext Context { get; set; }
        public IRequestSocket Socket { get; set; }

        public virtual void Connect(string endpoint, ZSocketType type)
        {
            Context = Context ?? new ZContext();
            Socket = Socket ?? new RequestSocketAdapter(new ZSocket(Context, type));
            Socket.Connect(endpoint);
        }
        
        public virtual void Send(string message)
        {
            Socket.Send(message);           
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