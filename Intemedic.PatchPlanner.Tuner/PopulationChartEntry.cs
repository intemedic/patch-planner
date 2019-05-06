namespace Intemedic.PatchPlanner.Tuner
{
    public class PopulationChartEntry
    {
        public PopulationChartEntry(int continentIndex, double fitness)
        {
            this.ContinentIndex = continentIndex;
            this.Fitness = fitness;
        }

        public int ContinentIndex { get; }
        public double Fitness { get; }
    }
}