using Rhyous.CS6210.Hw1.Interfaces;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class PublisherServer : IServer<ZFrame>, IReply, IDisposable
    {
        public ZContext Context { get; set; }
        public ISendSocketServer Socket { get; set; }

        public virtual async Task StartAsync(string endpoint, ZSocketType type, Action<ZFrame> receiveAction)
        {
           await Task.Run(() =>
           {
               Context = Context ?? new ZContext();
               Socket = Socket ?? new SendSocketServerAdapter(new ZSocket(Context, ZSocketType.PUB));
               Socket.Bind(endpoint);
           });
        }


        public virtual void Stop()
        {
            Dispose();
        }

        public virtual async Task ReplyAsync(string message)
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