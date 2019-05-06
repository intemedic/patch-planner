namespace Intemedic.PatchPlanner.Tuner
{
    public class FitnessChartEntry
    {
        public FitnessChartEntry(int generation, double fitness)
        {
            this.Generation = generation;
            this.Fitness = fitness;
        }

        public int Generation { get; }
        public double Fitness { get; }
    }
}