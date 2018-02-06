using System;

namespace Rhyous.CS6210.Hw1.Interfaces
{
    public interface IRecord : IEntity
    {
        int Disease { get; set; }
        DateTime DiagnosisDate { get; set; }
    }
}
