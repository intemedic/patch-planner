using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

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
            var penalty = 0.0;
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
                            penalty += intersection.GetArea();
                        }
                    }
                }

                if (patchCount == 0)
                {
                    penalty += annotations.SumArea / annotations.Count * 10;
                }
                else
                {
                    penalty += annotation.Bounds.GetArea() * (patchCount - 1);
                }
            }

            var x = -penalty / annotations.SumArea;

            this.Fitness = Math.Exp(x) / (Math.Exp(x) + 1) * 2;
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
            var majorMutate = Random.NextDouble() <= evolutionParameters.MajorMutationRate
                               / (evolutionParameters.MajorMutationRate + evolutionParameters.MinorMutationRate);

            var indexList = Enumerable.Range(0, this.Patches.Count).ToList();

            if (majorMutate)
            {
                Shuffle(indexList);

                var majorMutationMagnitude = evolutionParameters.MajorMutationMagnitude;
                if (evolutionParameters.AdaptiveMutation)
                {
                    majorMutationMagnitude /= this.Fitness;
                }

                var majorMutateCount = (int)Math.Round(majorMutationMagnitude * indexList.Count);
                foreach (var i in indexList.Take(majorMutateCount))
                {
                    this.Patches[i] = Patch.CreateRandom(Random);
                }
            }
            else
            {
                Shuffle(indexList);
                var minorMutateCount = (int)Math.Round(evolutionParameters.MinorMutationMagnitude * indexList.Count);
                foreach (var i in indexList.Take(minorMutateCount))
                {
                    var patch = this.Patches[i];
                    var newPosition = patch.Position
                                      + new Vector(
                                          Random.Next(Constants.PatchSize) - Constants.PatchSize / 2,
                                          Random.Next(Constants.PatchSize) - Constants.PatchSize / 2);

                    if (newPosition.X < 0)
                    {
                        newPosition.X = 0;
                    }
                    else if (newPosition.X > Constants.PatchPositionLimit)
                    {
                        newPosition.X = Constants.PatchPositionLimit;
                    }

                    if (newPosition.Y < 0)
                    {
                        newPosition.Y = 0;
                    }
                    else if (newPosition.Y > Constants.PatchPositionLimit)
                    {
                        newPosition.Y = Constants.PatchPositionLimit;
                    }

                    this.Patches[i] = new Patch(newPosition);
                }
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