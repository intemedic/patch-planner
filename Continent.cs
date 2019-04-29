using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Telerik.Charting;

namespace PatchPlanner
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class Continent : ViewModelBase
    {
        private Generation _currentGeneration;

        public Continent(
            int index,
            EvolutionParameters parameters,
            AnnotationCollection annotations)
        {
            this.Index = index;
            this.Parameters = parameters;
            this.Annotations = annotations;
        }


        private double _maxFitness;
        public double MaxFitness
        {
            get => _maxFitness;
            set
            {
                if (_maxFitness == value)
                {
                    return;
                }
                _maxFitness = value;

                this.RaisePropertyChanged(nameof(this.MaxFitness));
            }
        }

        private double _medianFitness;
        public double MedianFitness
        {
            get => _medianFitness;
            set
            {
                if (_medianFitness == value)
                {
                    return;
                }
                _medianFitness = value;

                this.RaisePropertyChanged(nameof(this.MedianFitness));
            }
        }



        public int Index { get; }
        public EvolutionParameters Parameters { get; }
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

        private Generation GenerateInitialGeneration()
        {
            var random = new Random();
            var generation = new Generation(this, 0);

            var patches = new List<Patch>();
            for (var patchIndex = 0; patchIndex < this.Parameters.InitialPatchCount; ++patchIndex)
            {
                patches.Add(Patch.CreateRandom(random));
            }

            var seedIndividual = new Individual(generation, patches);
            generation.Population.Add(seedIndividual);

            for (var i = 1; i < this.Parameters.Population; ++i)
            {
                generation.Population.Add(seedIndividual.Clone());
            }

            generation.OnPopulationGenerated();

            return generation;
        }

        public void Initialize()
        {
            var initialGeneration = this.GenerateInitialGeneration();
            this.CurrentGeneration = initialGeneration;
            this.CurrentGeneration.EvaluateFitness(this.Annotations, this.Parameters);
            this.UpdateFitnessRange();
        }

        public void Evolve()
        {
            this.CurrentGeneration = this.CurrentGeneration.Evolve(
                this.Annotations,
                this.Parameters);

            this.UpdateFitnessRange();
        }

        private void UpdateFitnessRange()
        {
            this.MaxFitness = this.CurrentGeneration.BestFit.Fitness;
            var population = this.CurrentGeneration.Population;
            this.MedianFitness = population.Count % 2 == 0
                ? population[population.Count / 2].Fitness
                : (population[population.Count / 2].Fitness + population[population.Count / 2 + 1].Fitness) / 2;
        }

    }
}