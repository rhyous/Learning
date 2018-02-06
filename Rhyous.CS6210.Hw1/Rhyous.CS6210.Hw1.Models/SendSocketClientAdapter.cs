using Rhyous.CS6210.Hw1.Interfaces;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class SendSocketClientAdapter : ISendSocketClient
    {
        public SendSocketClientAdapter(ZSocket socket)
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
        
        public void Send(string message)
        {
            Socket.Send(new ZFrame(message));
        }
        
        public void Dispose()
        {
            Socket.Dispose();
        }

        #region
        public static implicit operator ZSocket(SendSocketClientAdapter socketAdapter)
        {
            return socketAdapter.Socket;
        }

        public static implicit operator SendSocketClientAdapter(ZSocket socket)
        {
            return new SendSocketClientAdapter(socket);
        }
        #endregion
    }
}