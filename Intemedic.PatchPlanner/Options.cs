using CommandLine;

namespace Intemedic.PatchPlanner
{
    internal class Options
    {
        [Value(0, HelpText = "The input annotation description JSON file.", Required = true)]
        public string InputFile { get; set; }

        [Value(1, HelpText = "The output patch description JSON file.", Required = true)]
        public string OutputFile { get; set; }

        [Option('e', "evolution-config", HelpText = "The evolution configuration file.")]
        public string EvolutionConfigFile { get; set; }

        [Option('s', "patch-size", HelpText = "Size of the patch.", Default = 512)]
        public int PatchSize { get; set; }

        [Option("max-partition-size", HelpText = "Maximum size of a single partition.", Default = 2048)]
        public int MaxPartitionSize { get; set; }

        [Option('p', "print-statistics", HelpText = "Print coverage statistics information.")]
        public bool PrintCoverageStatistics { get; set; }
    }
}