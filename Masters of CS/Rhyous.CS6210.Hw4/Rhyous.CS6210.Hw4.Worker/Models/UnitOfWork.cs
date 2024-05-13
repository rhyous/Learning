using System;

namespace Rhyous.CS6210.Hw4
{
    public class UnitOfWork
    {
        public int Id { get; set; }
        public WorkStatus Status { get; set; }
        public double Progress { get; set; }
        public object Result { get; set; }
        public string AssociateFile { get; set; }
        public DateTime Assigned { get; set; }
    }
}
