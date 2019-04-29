namespace PatchPlanner
{
    public class ChartEntry
    {
        public int Generation { get; }
        public double Fitness { get; }

        public ChartEntry(int generation, double fitness)
        {
            this.Generation = generation;
            this.Fitness = fitness;
        }
    }
}