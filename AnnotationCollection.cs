using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PatchPlanner
{
    public class AnnotationCollection : IReadOnlyCollection<Annotation>
    {
        public AnnotationCollection(IEnumerable<Annotation> annotations)
        {
            this.Annotations = new List<Annotation>(annotations);
            this.SumArea = this.Annotations.Sum(a => a.Bounds.GetArea());
        }

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

        public int Count => this.Annotations.Count;
    }
}