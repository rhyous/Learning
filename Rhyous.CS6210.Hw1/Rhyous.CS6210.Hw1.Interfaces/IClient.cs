using System;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IClient : IConnect
    {
        void Send(string message, Action receiveAction);
    }

    public interface IClient<T> : IConnect
    {
        void Send(string message, Action<T> receiveAction);
    }

    public interface IClient<T1, T2> : IConnect
    {
        void Send(string message, Action<T1, T2> receiveAction);
    }
    
    public interface IClient<T1, T2, T3> : IConnect
    {
        void Send(string message, Action<T1, T2, T3> receiveAction);
    }

    public interface IClient<T1, T2, T3, T4> : IConnect
    {
        void Send(string message, Action<T1, T2, T3, T4> receiveAction);
    }

    public interface IClient<T1, T2, T3, T4, T5> : IConnect
    {
        void Send(string message, Action<T1, T2, T3, T4, T5> receiveAction);
    }
}