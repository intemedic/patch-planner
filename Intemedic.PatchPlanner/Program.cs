using CommandLine;

namespace Intemedic.PatchPlanner
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => new PatchPlanner(options).RunAsync().Wait());
        }
    }
}