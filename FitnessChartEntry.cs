namespace PatchPlanner
{
    public class FitnessChartEntry
    {
        public int Generation { get; }
        public double Fitness { get; }

        public FitnessChartEntry(int generation, double fitness)
        {
            this.Generation = generation;
            this.Fitness = fitness;
        }
    }
}