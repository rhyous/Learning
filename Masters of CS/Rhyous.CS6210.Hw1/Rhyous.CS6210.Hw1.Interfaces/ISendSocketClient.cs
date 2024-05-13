using System;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface ISendSocketClient : ISendAsync, IDisposable, IConnect
    {
    }
}