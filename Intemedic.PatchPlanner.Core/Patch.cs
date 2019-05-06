using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Intemedic.PatchPlanner
{
    [DebuggerDisplay("Patch: {" + nameof(Position) + "}")]
    public class Patch
    {
        public static int LimitPosition(int value)
        {
            return Math.Min(Constants.PatchPositionLimit, value);
        }

        public static Patch CreateRandom(Random random, AnnotationGrid annotationGrid)
        {
            return new Patch(annotationGrid.GetRandomPoint(random));
        }

        public Patch(Point position)
        {
            this.Position = position;
            this.Bounds = new Rectangle(position, new Size(Constants.PatchSize, Constants.PatchSize));
        }

        public Point Position { get; }
        public Rectangle Bounds { get; }

        public static Patch CreateRandomAround(
            Rectangle bounds, 
            Random random,
            AnnotationGrid annotationGrid)
        {
            bounds.Inflate(Constants.PatchSize, Constants.PatchSize);
            var position = annotationGrid.GetRandomPointWithin(bounds, random);

            return position == null ? null : new Patch(position.Value);
        }
    }
}
