using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Intemedic.PatchPlanner
{
    internal class PatchPlanner
    {
        private const string DefaultEvolutionConfigFile = "evolution.json";

        public PatchPlanner(Options options)
        {
            this.Options = options;
        }

        private Options Options { get; }

        public Task RunAsync()
        {
            var parameters = this.LoadEvolutionConfig();
            return Task.CompletedTask;
        }

        private EvolutionParameters LoadEvolutionConfig()
        {
            var path = this.Options.EvolutionConfigFile
                       ?? DefaultEvolutionConfigFile;

            if (!File.Exists(path))
            {
                Console.Error.WriteLine($"Unable to find evolution config file '{path}'");
                Environment.Exit(-1);
            }

            return JsonConvert.DeserializeObject<EvolutionParameters>(path);
        }
    }
}