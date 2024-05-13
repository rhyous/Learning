using Rhyous.CS6210.Hw1.Interfaces;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class PullSocketAdapter : IPullSocket
    {
        public PullSocketAdapter(ZSocket socket)
        {
            Socket = socket;
        }

        public ZSocket Socket { get; }

        public virtual void Bind(string endpoint)
        {
            Socket.Bind(endpoint);
        }
    
        public virtual ZFrame ReceiveFrame()
        {
            return Socket.ReceiveFrame();
        }

        public void Dispose()
        {
            Socket.Dispose();
        }


        #region
        public static implicit operator ZSocket(PullSocketAdapter socketAdapter)
        {
            return socketAdapter.Socket;
        }

        public static implicit operator PullSocketAdapter(ZSocket socket)
        {
            return new PullSocketAdapter(socket);
        }
        #endregion
    }
}