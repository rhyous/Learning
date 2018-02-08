using Rhyous.CS6210.Hw1.Interfaces;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class SubscribeSocketAdapter : ISubscribeSocket
    {
        public SubscribeSocketAdapter(ZSocket socket = null)
        {
            Socket = socket ?? new ZSocket(ZSocketType.SUB);
        }

        public ZSocket Socket { get; }

        public bool IsConnected { get; internal set; }

        public virtual void Connect(string endpoint)
        {
            Socket.Connect(endpoint);
            IsConnected = true;
        }
        
        public void Subscribe(string name)
        {
            Socket.Subscribe(name);
        }
        
        public void Dispose()
        {
            Socket.Dispose();
        }

        #region
        public static implicit operator ZSocket(SubscribeSocketAdapter socketAdapter)
        {
            return socketAdapter.Socket;
        }

        public static implicit operator SubscribeSocketAdapter(ZSocket socket)
        {
            return new SubscribeSocketAdapter(socket);
        }
        #endregion
    }
}