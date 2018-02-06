namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IConnect
    {
        void Connect(string endpoint);
        bool IsConnected { get; }
    }
}
