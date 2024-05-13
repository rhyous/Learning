using Rhyous.CS6210.Hw1.Models;
using System;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface ILogger
    {
        void WriteLine(string message);
        void WriteLine(string message, int id, DateTime? date = null);
        void WriteLine(string message, VectorTimeStamp vts);
    }
}