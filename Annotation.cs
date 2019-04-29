using System.Windows;

namespace PatchPlanner
{
    public class Annotation
    {
        public Annotation(Rect bounds)
        {
            this.Bounds = bounds;
        }

        public Rect Bounds { get; }
    }
}