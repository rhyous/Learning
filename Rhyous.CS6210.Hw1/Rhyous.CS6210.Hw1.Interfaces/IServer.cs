using System;
using System.Threading.Tasks;
using ZeroMQ;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IServer
    {
        void Start(string endpoint, ZSocketType type, Action receiveAction);
        void Stop();
    }

    public interface IServer<T>
    {
        Task StartAsync(string endpoint, ZSocketType type, Action<T> receiveAction);
        void Stop();
    }

    public interface IServer<T1, T2>
    {
        Task StartAsync(string endpoint, ZSocketType type, Action<T1, T2> receiveAction);
        void Stop();
    }

    public interface IServer<T1, T2, T3>
    {
        Task StartAsync(string endpoint, ZSocketType type, Action<T1, T2, T3> receiveAction);
        void Stop();
    }

    public interface IServer<T1, T2, T3, T4>
    {
        Task StartAsync(string endpoint, ZSocketType type, Action<T1, T2, T3, T4> receiveAction);
        void Stop();
    }

    public interface IServer<T1, T2, T3, T4, T5>
    {
        Task StartAsync(string endpoint, ZSocketType type, Action<T1, T2, T3, T4, T5> receiveAction);
        void Stop();
    }
}