using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;

namespace Intemedic.PatchPlanner.Tuner
{
    internal class EvolutionViewModel : ViewModelBase
    {
        private IEnumerable<FitnessChartEntry> _fitnessChartData;

        private IEnumerable<PopulationChartEntry> _populationChartData;

        public EvolutionViewModel(Evolution evolution)
        {
            this.Evolution = evolution;
            evolution.GenerationEvolved += this.OnGenerationEvolved;
        }

        public Evolution Evolution { get; }

        private List<FitnessChartEntry> WritableFitnessChartData { get; } = new List<FitnessChartEntry>();

        public IEnumerable<FitnessChartEntry> FitnessChartData
        {
            get => _fitnessChartData;
            private set
            {
                _fitnessChartData = value;
                this.RaisePropertyChanged(nameof(this.FitnessChartData));
            }
        }

        public IEnumerable<PopulationChartEntry> PopulationChartData
        {
            get => _populationChartData;
            private set
            {
                _populationChartData = value;
                this.RaisePropertyChanged(nameof(this.PopulationChartData));
            }
        }

        public Individual BestFit => this.Evolution.BestFit;
        public int GenerationIndex => this.Evolution.GenerationIndex;

        private double CurrentMaxFitness { get; set; }

        private void OnGenerationEvolved(object sender, EventArgs e)
        {
            this.RaisePropertyChanged(nameof(this.BestFit));
            this.RaisePropertyChanged(nameof(this.GenerationIndex));
            this.CurrentMaxFitness = this.BestFit?.Fitness ?? 0;
            this.UpdateChartData();
        }

        private void UpdateChartData()
        {
            if (this.Evolution.GenerationIndex
                * this.Evolution.Parameters.Population
                % Constants.ChartUpdateInterval
                != 0)
            {
                return;
            }

            this.WritableFitnessChartData.Add(
                new FitnessChartEntry(
                    this.Evolution.GenerationIndex,
                    this.CurrentMaxFitness));
            this.CurrentMaxFitness = 0;

            this.FitnessChartData = this.WritableFitnessChartData.ToArray();

            var populationChartData = new List<PopulationChartEntry>();
            foreach (var continent in this.Evolution.Continents)
            {
                populationChartData.AddRange(
                    continent.CurrentGeneration.Population
                       // .Where(p => p.Fitness > 0.1)
                        .Select(p =>
                            new PopulationChartEntry(continent.Index, p.Fitness)));
            }

            this.PopulationChartData = populationChartData;
        }
    }
}