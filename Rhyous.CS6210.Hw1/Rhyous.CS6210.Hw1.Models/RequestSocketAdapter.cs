using System;
using System.Threading;
using System.Threading.Tasks;
using Rhyous.CS6210.Hw1.Interfaces;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class RequestSocketAdapter : IRequestSocket
    {
        internal bool ThrowOnFailureToSend;
        internal bool RetryOnFailure;
        public RequestSocketAdapter(ZSocket socket, bool throwOnFailureToSend, bool retryOnFailure)
        {
            Socket = socket;
            ThrowOnFailureToSend = throwOnFailureToSend;
            RetryOnFailure = retryOnFailure;
        }

        public ZSocket Socket { get; }

        public bool IsConnected { get; internal set; }

        public virtual void Connect(string endpoint)
        {
            Socket.Connect(endpoint);
            IsConnected = true;
        }

        public virtual ZFrame ReceiveFrame()
        {
            Socket.ReceiveTimeout = TimeSpan.FromMilliseconds(10000);
            return Socket.ReceiveFrame(out ZError error);
        }

        public async Task SendAsync(string message)
        {
            await Task.Run(() => {
                try { Socket.Send(new ZFrame(message)); }
                catch (Exception e) { }
            });
        }

        public async Task SendAsync(string message, Action<ZFrame> receiveAction)
        {
            if (RetryOnFailure)
            {
                await SendWithRetryAsync(message, receiveAction);
                return;
            }

            await SendAsync(message);
            await Task.Run(() =>
            {
                using (ZFrame reply = ReceiveFrame())
                {
                    receiveAction(reply);
                }
            });
        }

        public async Task SendWithRetryAsync(string message, Action<ZFrame> receiveAction) { 
            int retries = 0;
            while (true)
            {
                Socket.Connect(Socket.LastEndpoint);
                await SendAsync(message);
                try
                {
                    var cts = new CancellationTokenSource(3000);
                    bool received = false;
                    await Task.Run(() =>
                    {
                        using (ZFrame reply = ReceiveFrame())
                        {
                            receiveAction(reply);
                            received = reply != null;
                        }
                    }, cts.Token);
                    if (received || retries == 3)
                        return;
                }
                catch (Exception e)
                {
                    
                    if (retries == 3)
                        throw e;
                }
                retries++;
            }
        }

        public void Dispose()
        {
            IsConnected = false;
            Socket.Dispose();
        }

        #region
        public static implicit operator ZSocket(RequestSocketAdapter socketAdapter)
        {
            return socketAdapter.Socket;
        }

        public static implicit operator RequestSocketAdapter(ZSocket socket)
        {
            return new RequestSocketAdapter(socket, true, true);
        }
        #endregion
    }
}