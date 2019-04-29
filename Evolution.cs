using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PatchPlanner
{
    internal class Evolution : ViewModelBase
    {
        private IEnumerable<FitnessChartEntry> _fitnessChartData;
        private IEnumerable<PopulationChartEntry> _populationChartData;

        public Evolution(
            EvolutionParameters parameters,
            AnnotationCollection annotations)
        {
            this.Parameters = parameters;
            this.Continents = Enumerable.Range(0, parameters.Continents)
                .Select(i => new Continent(i, parameters, annotations))
                .ToArray();
        }

        public EvolutionParameters Parameters { get; }

        public IReadOnlyCollection<Continent> Continents { get; }

        public IEnumerable<FitnessChartEntry> FitnessChartData
        {
            get => _fitnessChartData;
            private set
            {
                if (ReferenceEquals(_fitnessChartData, value))
                {
                    return;
                }

                _fitnessChartData = value;

                this.RaisePropertyChanged(nameof(this.FitnessChartData));
            }
        }

        public IEnumerable<PopulationChartEntry> PopulationChartData
        {
            get => _populationChartData;
            private set
            {
                if (ReferenceEquals(_populationChartData, value))
                {
                    return;
                }

                _populationChartData = value;

                this.RaisePropertyChanged(nameof(this.PopulationChartData));
            }
        }

        private Individual _bestFit;
        public Individual BestFit
        {
            get => _bestFit;
            set
            {
                if (_bestFit == value)
                {
                    return;
                }
                _bestFit = value;

                this.RaisePropertyChanged(nameof(this.BestFit));
            }
        }




        private int _generationIndex;
        public int GenerationIndex
        {
            get => _generationIndex;
            private set
            {
                if (_generationIndex == value)
                {
                    return;
                }
                _generationIndex = value;

                this.RaisePropertyChanged(nameof(this.GenerationIndex));
            }
        }



        public Task StartAsync()
        {
            Parallel.ForEach(this.Continents, c => c.Initialize());

            var maxFitness = this.Continents.Max(c => c.CurrentGeneration.BestFit.Fitness);

            var fitnessChartData = new List<FitnessChartEntry>();
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
                        colonist.CurrentGeneration.Colonize(colony.CurrentGeneration);
                    }
                }

                Parallel.ForEach(this.Continents, c => c.Evolve());

                foreach (var continent in this.Continents)
                {
                    if (continent.MaxFitness > maxFitness)
                    {
                        maxFitness = continent.CurrentGeneration.BestFit.Fitness;
                        this.BestFit = continent.CurrentGeneration.BestFit;
                    }
                }

                if (i * this.Parameters.Population % Constants.ChartUpdateInterval == 0)
                {
                    fitnessChartData.Add(new FitnessChartEntry(i, maxFitness));
                    maxFitness = 0;

                    this.FitnessChartData = fitnessChartData.ToArray();

                    var populationChartData = new List<PopulationChartEntry>();
                    foreach (var continent in this.Continents)
                    {
                        populationChartData.AddRange(
                            continent.CurrentGeneration.Population
                                .Where(p => p.Fitness > 0.1)
                                .Select(p =>
                                new PopulationChartEntry(continent.Index, p.Fitness)));
                    }

                    this.PopulationChartData = populationChartData;
                }

                this.RaisePropertyChanged(nameof(this.Continents));
            }

            fitnessChartData.Add(new FitnessChartEntry(this.Parameters.MaxGenerationCount - 1, maxFitness));

            this.FitnessChartData = fitnessChartData;

            return Task.CompletedTask;
        }
    }
}