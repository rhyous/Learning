using System;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IPublisherSocket : IBind, ISendAsync, IReceiveFrame, IDisposable
    {
    }
}