using System;
using System.Collections.Generic;
using System.Linq;

namespace PatchPlanner
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

        private Individual SelectFather()
        {
            return this.SelectIndividual();
        }

        public (Individual father, Individual mother) SelectParents()
        {
            var father = this.SelectFather();
            var mother = this.SelectMother(father);

            return (father, mother);
        }

        private Individual SelectMother(Individual father)
        {
            while (true)
            {
                var mother = this.SelectIndividual();
                if (mother != father)
                {
                    return mother;
                }
            }
        }

        public Individual SelectIndividual()
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