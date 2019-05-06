using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Intemedic.PatchPlanner
{
    public class Evolution
    {
        public Evolution(
            EvolutionParameters parameters,
            AnnotationCollection annotations)
        {
            this.Parameters = parameters;
            this.Continents = Enumerable.Range(0, parameters.Continents)
                .Select(i => new Continent(i, parameters, annotations))
                .ToArray();
        }

        public event EventHandler GenerationEvolved;

        public EvolutionParameters Parameters { get; }

        public IReadOnlyCollection<Continent> Continents { get; }

        public Individual BestFit { get; private set; }

        public int GenerationIndex { get; private set; }


        public Task StartAsync()
        {
            Parallel.ForEach(this.Continents, c => c.Initialize());
            this.BestFit = this.Continents.First().CurrentGeneration.BestFit;
            this.OnGenerationEvolved();

            var maxFitness = this.Continents.Max(c => c.CurrentGeneration.BestFit.Fitness);

            for (var i = 1; i < this.Parameters.MaxGenerationCount; ++i)
            {
                this.GenerationIndex = i;

                if (i > this.Parameters.ConquerStartingGeneration
                    && (i - this.Parameters.ConquerStartingGeneration) % this.Parameters.ConquerInterval == 0)
                {
                    var bestContinentFitness = double.MinValue;
                    var worstContinentFitness = double.MaxValue;
                    Continent colonist = null;
                    Continent colony = null;

                    foreach (var continent in this.Continents)
                    {
                        if (continent.CurrentGeneration.BestFit.Fitness > bestContinentFitness)
                        {
                            bestContinentFitness = continent.CurrentGeneration.BestFit.Fitness;
                            colonist = continent;
                        }

                        if (continent.CurrentGeneration.BestFit.Fitness < worstContinentFitness)
                        {
                            worstContinentFitness = continent.CurrentGeneration.BestFit.Fitness;
                            colony = continent;
                        }
                    }

                    if (colonist != colony)
                    {
                        Debug.Assert(colonist != null);
                        Debug.Assert(colony != null);
                        colonist.CurrentGeneration.Colonize(colony.CurrentGeneration);
                    }
                }

                Parallel.ForEach(this.Continents, c => c.Evolve());

                foreach (var continent in this.Continents)
                {
                    if (continent.CurrentGeneration.BestFit.Fitness > maxFitness)
                    {
                        maxFitness = continent.CurrentGeneration.BestFit.Fitness;
                        this.BestFit = continent.CurrentGeneration.BestFit;
                    }
                }

                this.OnGenerationEvolved();
            }

            return Task.CompletedTask;
        }

        private void OnGenerationEvolved()
        {
            this.GenerationEvolved?.Invoke(this, EventArgs.Empty);
        }
    }
}