using Rhyous.CS6210.Hw1.Models;
using System;

namespace Rhyous.CS6210.Hw1.Simulator
{
    public class DiseaseRecordGenerator
    {
        public Record Generate(DateTime current, Random random)
        {
            return new Record
            {
                Disease = random.Next(Disease.Instance.Count),
                DiagnosisDate = current               
            };
        }
    }
}
