using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PatchPlanner
{
    [DebuggerDisplay("Gen #{Index}: Best Fit={BestFit}")]
    public class Generation : ViewModelBase
    {
        public Generation(Continent continent, int index)
        {
            this.Continent = continent;
            this.Index = index;
        }

        public Continent Continent { get; }
        public int Index { get; }

        public IList<Individual> Population { get; private set; }
            = new List<Individual>();

        private Individual _bestFit;
        public Individual BestFit
        {
            get => _bestFit;
            private set
            {
                if (_bestFit == value)
                {
                    return;
                }
                _bestFit = value;

                this.RaisePropertyChanged(nameof(this.BestFit));
            }
        }

        public Generation Evolve(
            AnnotationCollection annotations,
            EvolutionParameters parameters)
        {
            var nextGeneration = new Generation(this.Continent, this.Index + 1);
            nextGeneration.Breed(this, parameters);
            nextGeneration.EvaluateFitness(annotations, parameters);

            return nextGeneration;
        }

        private void Breed(Generation parentGeneration, EvolutionParameters parameters)
        {
            var parentPopulation = parentGeneration.Population.OrderByDescending(i => i.Fitness).ToArray();

            var keepCount = (int)Math.Round(parameters.KeepRate * parentPopulation.Length);
            var keptIndividuals = parentPopulation.Take(keepCount);
            foreach (var keptIndividual in keptIndividuals)
            {
                this.Population.Add(keptIndividual);
            }

            var breedPool = new BreedPool(parentPopulation);

            var childrenCount = parameters.Population - keepCount;
            for (var i = 0; i < childrenCount; ++i)
            {
                var parent = breedPool.SelectParent();
                var child = parent.Clone();

                child.Mutate(parameters);
                this.Population.Add(child);
            }

            this.OnPopulationGenerated();
        }

        public double EvaluateFitness(AnnotationCollection annotations, EvolutionParameters parameters)
        {
            Parallel.ForEach(this.Population, individual => individual.EvaluateFitness(annotations, parameters));

            this.Population = this.Population.OrderByDescending(p => p.Fitness).ToArray();
            this.RaisePropertyChanged(nameof(this.Population));

            this.BestFit = this.Population[0];
            return this.BestFit.Fitness;
        }

        public void OnPopulationGenerated()
        {
            for (var i = 0; i < this.Population.Count; i++)
            {
                this.Population[i].Index = i;
            }
        }

        public void Colonize(Generation native)
        {
            native.Population = this.Population.Select(p => p.Clone()).ToArray();
            native.OnPopulationGenerated();
        }
    }
}