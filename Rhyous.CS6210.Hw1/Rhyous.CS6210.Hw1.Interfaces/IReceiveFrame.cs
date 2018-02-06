using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IReceiveFrame
    {
        ZFrame ReceiveFrame();
    }
}
