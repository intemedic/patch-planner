namespace Intemedic.PatchPlanner
{
    public class EvolutionParameters
    {
        public int InitialPatchCount { get; set; }
        public int Population { get; set; }
        public int Continents { get; set; }
        public int MaxGenerationCount { get; set; }
        public int ConquerInterval { get; set; }
        public int ConquerStartingGeneration { get; set; }
        public double KeepRate { get; set; }
        public double MajorMutationRate { get; set; }
        public double MinorMutationRate { get; set; }
        public double PatchCountMutationRate { get; set; }
        public double MajorMutationMagnitude { get; set; }
        public double MinorMutationMagnitude { get; set; }
        public bool AdaptiveMutation { get; set; }
        public InitializationStrategy InitializationStrategy { get; set; }
    }
}
