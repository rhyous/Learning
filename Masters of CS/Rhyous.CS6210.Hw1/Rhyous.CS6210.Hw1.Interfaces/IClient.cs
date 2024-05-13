using System;
using System.Threading.Tasks;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IClient : IConnect
    {
        Task SendAsync(string message, Action receiveAction);
    }

    public interface IClient<T> : IConnect
    {
        Task SendAsync(string message, Action<T> receiveAction);
    }

    public interface IClient<T1, T2> : IConnect
    {
        Task SendAsync(string message, Action<T1, T2> receiveAction);
    }
    
    public interface IClient<T1, T2, T3> : IConnect
    {
        Task SendAsync(string message, Action<T1, T2, T3> receiveAction);
    }

    public interface IClient<T1, T2, T3, T4> : IConnect
    {
        Task SendAsync(string message, Action<T1, T2, T3, T4> receiveAction);
    }

    public interface IClient<T1, T2, T3, T4, T5> : IConnect
    {
        Task SendAsync(string message, Action<T1, T2, T3, T4, T5> receiveAction);
    }
}