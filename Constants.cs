namespace PatchPlanner
{
    internal static class Constants
    {
        public const int PatchSize = 256;
        public const int PatchPositionLimit = 1024;
        public const int CanvasSize = PatchPositionLimit + PatchSize;

        public const int FitnessChartUpdateInterval = 500;
    }
}