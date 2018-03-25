using System;
using System.Threading.Tasks;
using Rhyous.CS6210.Hw1.Interfaces;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class RequestSocketAdapter : IRequestSocket
    {
        public RequestSocketAdapter(ZSocket socket)
        {
            Socket = socket;
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
            return Socket.ReceiveFrame();
        }
        
        public async Task SendAsync(string message)
        {
            await Task.Run(() => { Socket.Send(new ZFrame(message)); });
        }

        public async Task SendAsync(string message, Action<ZFrame> receiveAction)
        {
            await SendAsync(message);
            using (ZFrame reply = ReceiveFrame())
            {
                receiveAction(reply);
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
            return new RequestSocketAdapter(socket);
        }
        #endregion
    }
}