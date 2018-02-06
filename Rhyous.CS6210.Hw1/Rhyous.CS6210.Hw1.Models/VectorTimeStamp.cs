using Rhyous.CS6210.Hw1.Interfaces;

namespace Rhyous.CS6210.Hw1.Models
{
    public class VectorTimeStamp : IVectorTimeStamp
    {
        public VectorTimeStamp(int simulator, int healthDistrict, int analyzer)
        {
            Simulator = simulator;
            HealthDistrict = healthDistrict;
            Analyzer = analyzer;
        }

        public int Simulator { get; set; }
        public int HealthDistrict { get; set; }
        public int Analyzer { get; set; }

        public override string ToString()
        {
            return $"[{Simulator},{HealthDistrict},{Analyzer}]";
        }
    }
}