using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatchPlanner
{
    internal class Evolution : ViewModelBase
    {
        private IEnumerable<ChartEntry> _chartData;
        private Generation _currentGeneration;


        public Evolution(
            EvolutionParameters evolutionParameters,
            AnnotationCollection annotations)
        {
            this.EvolutionParameters = evolutionParameters;
            this.Annotations = annotations;
        }

        public IEnumerable<ChartEntry> ChartData
        {
            get => _chartData;
            private set
            {
                if (ReferenceEquals(_chartData, value))
                {
                    return;
                }

                _chartData = value;

                this.RaisePropertyChanged(nameof(this.ChartData));
            }
        }

        public EvolutionParameters EvolutionParameters { get; }
        public AnnotationCollection Annotations { get; }

        public Generation CurrentGeneration
        {
            get => _currentGeneration;
            private set
            {
                if (_currentGeneration == value)
                {
                    return;
                }

                _currentGeneration = value;

                this.RaisePropertyChanged(nameof(this.CurrentGeneration));
            }
        }

        public async Task StartAsync()
        {
            var initialGeneration = this.GenerateInitialGeneration();
            this.CurrentGeneration = initialGeneration;
            var fitness = this.CurrentGeneration.EvaluateFitness(this.Annotations);
            var chartData = new List<ChartEntry> {new ChartEntry(0, fitness)};

            var maxFitness = fitness;

            for (var i = 1; i < this.EvolutionParameters.MaxGenerationCount; ++i)
            {
                var previousGeneration = this.CurrentGeneration;
                this.CurrentGeneration = await previousGeneration.EvoluteAsync(
                    this.Annotations,
                    this.EvolutionParameters);

                fitness = this.CurrentGeneration.BestFit.Fitness;
                if (fitness > maxFitness)
                {
                    maxFitness = fitness;
                }

                if (i % Constants.FitnessChartUpdateInterval == 0)
                {
                    chartData.Add(new ChartEntry(i, maxFitness));
                    maxFitness = 0;

                    this.ChartData = chartData.ToArray();
                }
            }

            chartData.Add(new ChartEntry(this.EvolutionParameters.MaxGenerationCount - 1, maxFitness));

            this.ChartData = chartData;
        }

        private Generation GenerateInitialGeneration()
        {
            var random = new Random();
            var generation = new Generation(0);

            for (var individualIndex = 0; individualIndex < this.EvolutionParameters.Population; ++individualIndex)
            {
                var patches = new List<Patch>();
                for (var patchIndex = 0; patchIndex < this.EvolutionParameters.PatchCount; ++patchIndex)
                {
                    patches.Add(Patch.CreateRandom(random));
                }

                generation.Population.Add(new Individual(patches));
            }

            generation.OnPopulationGenerated();

            return generation;
        }
    }
}