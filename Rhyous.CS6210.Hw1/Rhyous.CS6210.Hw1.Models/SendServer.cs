using Rhyous.CS6210.Hw1.Interfaces;
using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class SendServer : IServer<ZFrame>, ISend, IDisposable
    {
        public ZContext Context { get; set; }
        public IReplySocket Socket { get; set; }

        public virtual void Start(string endpoint, ZSocketType type, Action<ZFrame> receiveAction)
        {            
            Context = Context ?? new ZContext();
            Socket = Socket ?? new SendSocketServerAdapter(new ZSocket(Context, type));
            Socket.Bind(endpoint);
            while (!_IsDisposed)
            {
                OpenReceive(receiveAction);
            }
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

        public virtual void Send(string message)
        {
            Socket.Send(message);
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