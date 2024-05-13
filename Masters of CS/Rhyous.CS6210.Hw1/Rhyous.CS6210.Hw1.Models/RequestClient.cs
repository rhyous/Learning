using Rhyous.CS6210.Hw1.Interfaces;
using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class RequestClient : IClient<ZFrame>, IDisposable
    {
        private bool ThrowOnFailureToSend;
        private bool RetryOnFailure;
        public RequestClient(bool throwOnFailureToSend, bool retryOnFailure)
        {
            ThrowOnFailureToSend = throwOnFailureToSend;
            RetryOnFailure = retryOnFailure;
        }

        public ZContext Context { get; set; }
        public IRequestSocket Socket { get; set; }
        public bool IsConnected { get; internal set; }

        public void Connect(string endpoint)
        {
            Context = Context ?? new ZContext();
            Socket = Socket ?? new RequestSocketAdapter(new ZSocket(Context, ZSocketType.REQ), ThrowOnFailureToSend, RetryOnFailure);

            // Connect
            Socket.Connect(endpoint);
            IsConnected = true;
        }
        
        public async Task SendAsync(string message, Action<ZFrame> receiveAction)
        {
            await Socket.SendAsync(message, receiveAction);           
        }

        #region IDisposable

        private bool _IsDisposed;

        public void Dispose()
        {
            IsConnected = false;
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