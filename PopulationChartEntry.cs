namespace PatchPlanner
{
    public class PopulationChartEntry
    {
        public int ContinentIndex { get; }
        public double Fitness { get; }

        public PopulationChartEntry(int continentIndex, double fitness)
        {
            this.ContinentIndex = continentIndex;
            this.Fitness = fitness;
        }
    }
}