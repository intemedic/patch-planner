using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Intemedic.PatchPlanner.Utilities;
namespace Intemedic.PatchPlanner
{
    public class AnnotationCollection : IReadOnlyCollection<Annotation>
    {
        public AnnotationCollection(IEnumerable<Annotation> annotations)
        {
            this.Annotations = new List<Annotation>(annotations);
            this.SumArea = this.Annotations.Sum(a => a.Bounds.GetArea());

            this.QuadTree = new QuadTree<Annotation>();

            foreach (var annotation in this.Annotations)
            {
                this.QuadTree.Insert(annotation, annotation.Bounds);
            }

            this.AnnotationGrid = new AnnotationGrid(this.Annotations);
        }

        public AnnotationGrid AnnotationGrid { get; }

        private QuadTree<Annotation> QuadTree { get; }

        private List<Annotation> Annotations { get; }

        public double SumArea { get; }

        public IEnumerator<Annotation> GetEnumerator()
        {
            return this.Annotations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerable<Annotation> EnumerateIntersectedAnnotations(Rectangle bounds)
        {
            return this.QuadTree.GetIntersectingElements(bounds);
        }

        public IEnumerable<Annotation> EnumerateContainingAnnotations(Rectangle bounds)
        {
            return this.QuadTree.GetContainingElements(bounds);
        }

        public int Count => this.Annotations.Count;

        
    }
}