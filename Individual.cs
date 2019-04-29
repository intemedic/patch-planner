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
        public Individual(Generation generation, IList<Patch> patches)
        {
            this.Generation = generation;
            this.Patches = patches;
        }

        public Generation Generation { get; }
        public IList<Patch> Patches { get; }

        public int PatchCount => this.Patches.Count;

        public double Fitness { get; private set; }

        private static Random Random { get; } = new Random();
        public int Index { get; set; }

        public void EvaluateFitness(AnnotationCollection annotations, EvolutionParameters parameters)
        {
            if (this.Patches.Count == 0)
            {
                this.Fitness = 0;
                return;
            }

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

            penalty *= 1 + ((double)this.Patches.Count / parameters.InitialPatchCount);

            var x = -penalty / annotations.SumArea;

            this.Fitness = Math.Exp(x) / (Math.Exp(x) + 1) * 2;
        }

        public void Mutate(EvolutionParameters parameters)
        {
            var sumMutationRate = parameters.MajorMutationRate
                                  + parameters.MinorMutationRate
                                  + parameters.PatchCountMutationRate;

            var roll = Random.NextDouble() * sumMutationRate;


            if (roll <= parameters.MajorMutationRate)
            {
                this.MajorMutate(parameters);
            }
            else if (roll <= parameters.MajorMutationRate + parameters.MinorMutationRate)
            {
                this.MinorMutate(parameters);
            }
            else
            {
                this.MutatePatchCount(parameters);
            }
        }

        private void MutatePatchCount(EvolutionParameters parameters)
        {
            var increase = Random.NextDouble() >= 0.5;
            if (increase)
            {
                this.Patches.Add(Patch.CreateRandom(Random));
            }
            else
            {
                this.Patches.RemoveAt(Random.Next(this.Patches.Count));
            }
        }

        private void MinorMutate(EvolutionParameters parameters)
        {
            var indexList = this.GenerateShuffledIndexList();
            var minorMutateCount = (int)Math.Round(parameters.MinorMutationMagnitude * indexList.Count);
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

        private List<int> GenerateShuffledIndexList()
        {
            var indexList = Enumerable.Range(0, this.Patches.Count).ToList();
            Shuffle(indexList);
            return indexList;
        }

        private void MajorMutate(EvolutionParameters parameters)
        {
            var indexList = this.GenerateShuffledIndexList();

            var majorMutationMagnitude = parameters.MajorMutationMagnitude;
            if (parameters.AdaptiveMutation)
            {
                majorMutationMagnitude /= this.Fitness;
            }

            var majorMutateCount = (int)Math.Round(majorMutationMagnitude * indexList.Count);
            foreach (var i in indexList.Take(majorMutateCount))
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
            return new Individual(
                this.Generation,
                new List<Patch>(this.Patches))
            {
                Fitness = this.Fitness
            };
        }
    }
}