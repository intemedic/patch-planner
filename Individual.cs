using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PatchPlanner
{
    [DebuggerDisplay("Fitness={" + nameof(Fitness) + "}")]
    public class Individual
    {
        public Individual(IList<Patch> patches)
        {
            this.Patches = patches;
        }

        public IList<Patch> Patches { get; }

        public double Fitness { get; private set; }

        private static Random Random { get; } = new Random();
        public int Index { get; set; }

        public void EvaluateFitness(AnnotationCollection annotations)
        {
            var score = 0.0;
            foreach (var annotation in annotations)
            {
                var patchCount = 0;
                foreach (var patch in this.Patches)
                {
                    if (patch.Bounds.IntersectsWith(annotation.Bounds))
                    {
                        if (patch.Bounds.Contains(annotation.Bounds))
                        {
                            ++patchCount;
                        }
                        else
                        {
                            var intersection = patch.Bounds;
                            intersection.Intersect(annotation.Bounds);
                            score += intersection.GetArea();
                        }
                    }
                }

                if (patchCount == 0)
                {
                    score += annotation.Bounds.GetArea() * 10;
                }
                else
                {
                    score += annotation.Bounds.GetArea() * (patchCount - 1);
                }
            }

            this.Fitness = 1 - score / (annotations.SumArea * 5);
        }

        public static Individual Crossover(Individual father, Individual mother)
        {
            var patchesCount = father.Patches.Count;
            var crossoverPoint = Random.Next(patchesCount);
            var fatherPatches = father.Patches.OrderBy(p => p.Position.X).Take(crossoverPoint);
            var motherPatches = mother.Patches.OrderBy(p => p.Position.X).Skip(crossoverPoint);

            var childPatches = fatherPatches
                .Concat(motherPatches)
                .ToArray();

            var fitness = (father.Fitness * crossoverPoint
                           + mother.Fitness * (patchesCount - crossoverPoint))
                          / patchesCount;

            return new Individual(childPatches)
            {
                Fitness = fitness
            };
        }

        public void Mutate(EvolutionParameters evolutionParameters)
        {
            var indexList = Enumerable.Range(0, this.Patches.Count).ToList();
            Shuffle(indexList);

            var mutateRate = evolutionParameters.MutationRate;
            if (evolutionParameters.AdaptiveMutation)
            {
                mutateRate /= this.Fitness;
            }

            var mutateCount = (int)Math.Round(mutateRate * indexList.Count);
            foreach (var i in indexList.Take(mutateCount))
            {
                this.Patches[i] = Patch.CreateRandom(Random);
            }
        }

        private static void Shuffle<T>(IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = Random.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public Individual Clone()
        {
            return new Individual(new List<Patch>(this.Patches))
            {
                Fitness = this.Fitness
            };
        }
    }
}