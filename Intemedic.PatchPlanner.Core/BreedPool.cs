using System;
using System.Collections.Generic;
using System.Linq;

namespace Intemedic.PatchPlanner
{
    internal class BreedPool
    {
        public BreedPool(IEnumerable<Individual> individuals)
        {
            this.Individuals = individuals.ToArray();
            this.SumFitness = this.Individuals.Sum(i => i.Fitness);
            this.Random = new Random();
        }

        private Random Random { get; }

        public IReadOnlyList<Individual> Individuals { get; }
        public double SumFitness { get; }


        public Individual SelectParent()
        {
            var value = this.Random.NextDouble() * this.SumFitness;

            var sumFitness = 0.0;
            foreach (var individual in this.Individuals)
            {
                sumFitness += individual.Fitness;
                if (sumFitness >= value)
                {
                    return individual;
                }
            }

            return this.Individuals[this.Individuals.Count - 1];
        }
    }
}