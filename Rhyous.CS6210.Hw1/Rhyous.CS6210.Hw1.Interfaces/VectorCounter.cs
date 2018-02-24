using System;

namespace Rhyous.CS6210.Hw1.Models
{
    public class VectorCounter
    {
        public int Counter;
        public DateTime Date;
        public override string ToString()
        {
            return $"{Counter}:{Date}";
        }
    }
}