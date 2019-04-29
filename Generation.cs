using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PatchPlanner
{
    [DebuggerDisplay("Gen #{Index}: Best Fit={BestFit}")]
    internal class Generation : ViewModelBase
    {
        public Generation(int index)
        {
            this.Index = index;
        }

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

        public async Task<Generation> EvoluteAsync(
            AnnotationCollection annotations,
            EvolutionParameters evolutionParameters)
        {
            var nextGeneration = new Generation(this.Index + 1);
            nextGeneration.Breed(this, evolutionParameters);
            await Task.Run(() => nextGeneration.EvaluateFitness(annotations));

            return nextGeneration;
        }

        private void Breed(Generation parentGeneration, EvolutionParameters evolutionParameters)
        {
            var parentPopulation = parentGeneration.Population.OrderByDescending(i => i.Fitness).ToArray();

            var keepCount = (int)Math.Round(evolutionParameters.KeepRate * parentPopulation.Length);
            var keptIndividuals = parentPopulation.Take(keepCount);
            foreach (var keptIndividual in keptIndividuals)
            {
                this.Population.Add(keptIndividual);
            }

            var breedPool = new BreedPool(parentPopulation);

            var childrenCount = evolutionParameters.Population - keepCount;
            for (var i = 0; i < childrenCount; ++i)
            {
                Individual child;
                if (evolutionParameters.Monogenesis)
                {
                    var father = breedPool.SelectIndividual();
                    child = father.Clone();
                }
                else
                {
                    var (father, mother) = breedPool.SelectParents();
                    child = Individual.Crossover(father, mother);
                }


                child.Mutate(evolutionParameters);
                this.Population.Add(child);
            }

            this.OnPopulationGenerated();
        }

        public double EvaluateFitness(AnnotationCollection annotations)
        {
            Parallel.ForEach(this.Population, individual => individual.EvaluateFitness(annotations));

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
    }
}