using Rhyous.CS6210.Hw1.Interfaces;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Models
{
    public class SendSocketServerAdapter : IReplySocket
    {
        public SendSocketServerAdapter(ZSocket socket)
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

        public async Task SendAsync(string message)
        {
            await Task.Run(() => { Socket.Send(new ZFrame(message)); });
        }

        public void Dispose()
        {
            Socket.Dispose();
        }


        #region
        public static implicit operator ZSocket(SendSocketServerAdapter socketAdapter)
        {
            return socketAdapter.Socket;
        }

        public static implicit operator SendSocketServerAdapter(ZSocket socket)
        {
            return new SendSocketServerAdapter(socket);
        }
        #endregion
    }
}