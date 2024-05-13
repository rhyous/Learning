using Rhyous.CS6210.Hw1.Interfaces;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class SendServer : IServer<ZFrame>, ISendAsync, IDisposable
    {
        public ZContext Context { get; set; }
        public IReplySocket Socket { get; set; }
        public bool IsStarted { get; internal set; }

        public virtual async Task StartAsync(string endpoint, ZSocketType type, Action<ZFrame> receiveAction)
        {            
            Context = Context ?? new ZContext();
            Socket = Socket ?? new SendSocketServerAdapter(new ZSocket(Context, type));
            Socket.Bind(endpoint);
            while (!_IsDisposed)
            {
                await OpenReceiveAsync(receiveAction);
            }
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

        public virtual async Task SendAsync(string message)
        {
            await Socket.SendAsync(message);
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