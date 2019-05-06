using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Intemedic.PatchPlanner.Utilities;

namespace Intemedic.PatchPlanner
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

        private Random Random { get; } = new Random();
        public int Index { get; set; }

        private IReadOnlyList<Annotation> UncoveredAnnotations { get; set; }

        public void EvaluateFitness(AnnotationCollection annotations, EvolutionParameters parameters)
        {
            if (this.Patches.Count == 0)
            {
                this.Fitness = 0;
                return;
            }

            var annotationOwnerCountMap = new Dictionary<Annotation, int>();

            var penalty = 0.0;

            foreach (var patch in this.Patches)
            {
                var intersectedAnnotations = annotations.EnumerateIntersectedAnnotations(patch.Bounds);

                foreach (var annotation in intersectedAnnotations)
                {
                    if (patch.Bounds.Contains(annotation.Bounds))
                    {
                        if (annotationOwnerCountMap.TryGetValue(annotation, out var patchCount))
                        {
                            annotationOwnerCountMap[annotation] = patchCount + 1;
                        }
                        else
                        {
                            annotationOwnerCountMap[annotation] = 1;
                        }
                    }
                    else
                    {
                        var intersection = patch.Bounds;
                        intersection.Intersect(annotation.Bounds);
                        penalty += intersection.GetArea() * 3;
                    }
                }
            }

            var uncoveredAnnotations = new List<Annotation>();

            foreach (var annotation in annotations)
            {
                if (!annotationOwnerCountMap.TryGetValue(annotation, out var patchCount))
                {
                    penalty += annotations.SumArea / annotations.Count * 10;
                    uncoveredAnnotations.Add(annotation);
                }
                else if (patchCount > 1)
                {
                    penalty += annotation.Bounds.GetArea() * (patchCount - 1);
                }
            }

            this.UncoveredAnnotations = uncoveredAnnotations;

            penalty *= 1 + (double)this.Patches.Count / parameters.InitialPatchCount;

            var x = -penalty / annotations.SumArea;

            this.Fitness = Math.Exp(x) / (Math.Exp(x) + 1) * 2;
        }

        public void Mutate(EvolutionParameters parameters, AnnotationCollection annotations)
        {
            var sumMutationRate = parameters.MajorMutationRate
                                  + parameters.MinorMutationRate
                                  + parameters.PatchCountMutationRate;

            var roll = this.Random.NextDouble() * sumMutationRate;

            if (roll <= parameters.MajorMutationRate)
            {
                this.MajorMutate(parameters, annotations);
            }
            else if (roll <= parameters.MajorMutationRate + parameters.MinorMutationRate)
            {
                this.MinorMutate(parameters, annotations);
            }
            else
            {
                this.MutatePatchCount(parameters, annotations);
            }

            this.Optimize();
        }

        private void Optimize()
        {

        }

        private void MutatePatchCount(EvolutionParameters parameters, AnnotationCollection annotations)
        {
            var increase = this.Random.NextDouble() >= 0.5;
            if (increase)
            {
                if (this.UncoveredAnnotations.Count > 0)
                {
                    var annotation = this.UncoveredAnnotations[this.Random.Next(this.UncoveredAnnotations.Count)];
                    this.Patches.Add(Patch.CreateRandomAround(annotation.Bounds, this.Random, annotations.AnnotationGrid));
                }
                else
                {
                    this.Patches.Add(Patch.CreateRandom(this.Random, annotations.AnnotationGrid));
                }
            }
            else
            {
                this.Patches.RemoveAt(this.Random.Next(this.Patches.Count));
            }
        }

        private void MinorMutate(EvolutionParameters parameters, AnnotationCollection annotations)
        {
            var indexList = this.GenerateShuffledIndexList();
            var minorMutateCount = (int)Math.Round(parameters.MinorMutationMagnitude * indexList.Count);
            if (minorMutateCount == 0)
            {
                minorMutateCount = 1;
            }

            foreach (var i in indexList.Take(minorMutateCount))
            {
                var patch = this.Patches[i];
                var newPosition = patch.Position;

                newPosition.Offset(this.Random.Next(Constants.PatchSize) - Constants.PatchSize / 2, this.Random.Next(Constants.PatchSize) - Constants.PatchSize / 2);

                if (newPosition.X < 0)
                {
                    newPosition.X = 0;
                }

                if (newPosition.Y < 0)
                {
                    newPosition.Y = 0;
                }

                newPosition.X = Patch.LimitPosition(newPosition.X);
                newPosition.Y = Patch.LimitPosition(newPosition.Y);

                this.Patches[i] = new Patch(newPosition);
            }
        }

        private List<int> GenerateShuffledIndexList()
        {
            var indexList = Enumerable.Range(0, this.Patches.Count).ToList();
            indexList.Shuffle(this.Random);
            return indexList;
        }

        private void MajorMutate(EvolutionParameters parameters, AnnotationCollection annotations)
        {
            var indexList = this.GenerateShuffledIndexList();

            var majorMutationMagnitude = parameters.MajorMutationMagnitude;
            if (parameters.AdaptiveMutation)
            {
                majorMutationMagnitude /= this.Fitness;
            }

            var majorMutateCount = (int)Math.Round(majorMutationMagnitude * indexList.Count);
            if (majorMutateCount == 0)
            {
                majorMutateCount = 1;
            }

            foreach (var i in indexList.Take(majorMutateCount))
            {
                this.Patches[i] = Patch.CreateRandom(this.Random, annotations.AnnotationGrid);
            }
        }


        public Individual Clone()
        {
            return new Individual(
                this.Generation,
                new List<Patch>(this.Patches))
            {
                Fitness = this.Fitness,
                UncoveredAnnotations = this.UncoveredAnnotations
            };
        }
    }
}