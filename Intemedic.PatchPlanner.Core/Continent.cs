using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Intemedic.PatchPlanner.Utilities;

namespace Intemedic.PatchPlanner
{
    public class Continent
    {
        public Continent(
            int index,
            EvolutionParameters parameters,
            AnnotationCollection annotations)
        {
            this.Index = index;
            this.Parameters = parameters;
            this.Annotations = annotations;
        }

        public int Index { get; }
        public EvolutionParameters Parameters { get; }
        public AnnotationCollection Annotations { get; }

        public Generation CurrentGeneration { get; private set; }

        private Generation GenerateInitialGeneration()
        {
            var generation = new Generation(this, 0);

            IList<Patch> patches;
            switch (this.Parameters.InitializationStrategy)
            {
                case InitializationStrategy.Random:
                    patches = this.GenerateRandomPatches();
                    break;
                case InitializationStrategy.RandomGreedy:
                    patches = this.GenerateRandomGreedyPatches();
                    break;
                case InitializationStrategy.Greedy:
                    patches = this.GenerateGreedyPatches();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

        private IList<Patch> GenerateRandomPatches()
        {
            var random = new Random();
            var patches = new List<Patch>();
            for (var patchIndex = 0; patchIndex < this.Parameters.InitialPatchCount; ++patchIndex)
            {
                patches.Add(Patch.CreateRandom(random, this.Annotations.AnnotationGrid));
            }

            return patches;
        }

        private IList<Patch> GenerateRandomGreedyPatches()
        {
            var patches = new List<Patch>();
            var random = new Random();

            var annotationList = this.Annotations.ToList();
            annotationList.Shuffle(random);

            var annotations = new HashSet<Annotation>(annotationList);

            while (annotations.Count > 0)
            {
                var seed = annotations.First();
                var patchX = Patch.LimitPosition(seed.Bounds.X);
                var patchY = Patch.LimitPosition(seed.Bounds.Y);


                if (patchX > Constants.PatchPositionLimit)
                {
                    patchX = Constants.PatchPositionLimit;
                }

                if (patchY > Constants.PatchPositionLimit)
                {
                    patchY = Constants.PatchPositionLimit;
                }

                var patchBounds = new Rectangle(
                    patchX,
                    patchY,
                    Constants.PatchSize,
                    Constants.PatchSize);

                var containedAnnotations = this.Annotations.EnumerateContainingAnnotations(patchBounds);

                foreach (var annotation in containedAnnotations)
                {
                    annotations.Remove(annotation);
                }


                patches.Add(new Patch(new Point(patchX, patchY)));
            }

            return patches;
        }

        private IList<Patch> GenerateGreedyPatches()
        {
            var patches = new List<Patch>();
            var xSteps = this.Annotations.Select(a => a.Bounds.X)
                .Distinct()
                .OrderBy(x => x)
                .ToArray();

            var ySteps = this.Annotations.Select(a => a.Bounds.Y)
                .Distinct()
                .OrderBy(y => y)
                .ToArray();

            var containedAnnotations = new HashSet<Annotation>();

            foreach (var y in ySteps)
            {
                foreach (var x in xSteps)
                {
                    var patchBounds = new Rectangle(
                        x,
                        y,
                        Constants.PatchSize,
                        Constants.PatchSize);

                    var annotations = this.Annotations.EnumerateContainingAnnotations(patchBounds)
                        .Where(a => !containedAnnotations.Contains(a))
                        .ToArray();

                    if (annotations.Length > 0)
                    {
                        var patchX = Patch.LimitPosition(annotations.Min(a => a.Bounds.X));
                        var patchY = Patch.LimitPosition(annotations.Min(a => a.Bounds.Y));

                        patches.Add(new Patch(new Point(patchX, patchY)));

                        patchBounds = new Rectangle(
                            patchX,
                            patchY,
                            Constants.PatchSize,
                            Constants.PatchSize);

                        foreach (var annotation in this.Annotations.EnumerateContainingAnnotations(patchBounds))
                        {
                            containedAnnotations.Add(annotation);
                        }
                    }
                }
            }

            return patches;
        }

        public void Initialize()
        {
            var initialGeneration = this.GenerateInitialGeneration();
            this.CurrentGeneration = initialGeneration;
            this.CurrentGeneration.EvaluateFitness(this.Annotations, this.Parameters);
        }

        public void Evolve()
        {
            this.CurrentGeneration = this.CurrentGeneration.Evolve(
                this.Annotations,
                this.Parameters);
        }
    }
}