using System.Diagnostics;
using System.Drawing;

namespace Intemedic.PatchPlanner
{
    [DebuggerDisplay("Annotation: {" + nameof(Bounds) + "}")]
    public class Annotation
    {
        public Annotation(Rectangle bounds)
        {
            this.Bounds = bounds;
        }

        public Rectangle Bounds { get; }
    }
}