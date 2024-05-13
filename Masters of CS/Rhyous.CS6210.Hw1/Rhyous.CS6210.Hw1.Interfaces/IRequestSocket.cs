using System;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IRequestSocket : IClient<ZFrame>, ISendSocketClient, IReceiveFrame
    {
    }
}
