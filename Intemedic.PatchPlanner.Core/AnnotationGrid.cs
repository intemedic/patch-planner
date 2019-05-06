using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Intemedic.PatchPlanner
{
    public class AnnotationGrid
    {

        public AnnotationGrid(IEnumerable<Annotation> annotations)
        {
            var topLeft = new Point(0, 0);
            var topRight = new Point(0, Constants.PatchPositionLimit);
            var bottomLeft = new Point(Constants.PatchPositionLimit, 0);
            var bottomRight = new Point(Constants.PatchPositionLimit, Constants.PatchPositionLimit);

            var horizontalLines = new List<LineSegment>
            {
                new LineSegment(topLeft, topRight),
                new LineSegment(bottomLeft, bottomRight)
            };

            var verticalLines = new List<LineSegment>
            {
                new LineSegment(topLeft, bottomLeft),
                new LineSegment(topRight, bottomRight)
            };

            foreach (var annotation in annotations)
            {
                var end = annotation.Bounds.Location;

                var horizontalStart = end;
                horizontalStart.Offset(-Constants.PatchSize, 0);

                var verticalStart = end;
                verticalStart.Offset(0, -Constants.PatchSize);

                horizontalLines.Add(new LineSegment(horizontalStart, end));
                verticalLines.Add(new LineSegment(verticalStart, end));
            }

            this.LineSegments = horizontalLines.Concat(verticalLines).ToArray();

            this.Intersections = horizontalLines.SelectMany(
                    h => verticalLines.Select(v => v.GetIntersectionWith(h)))
                .Where(i => i != null)
                .Select(i => i.Value)
                .Where(i => i.X >= 0
                            && i.X <= Constants.PatchPositionLimit
                            && i.Y >= 0
                            && i.Y <= Constants.PatchPositionLimit)
                .Distinct()
                .ToArray();
        }

        public LineSegment[] LineSegments { get; }
        public Point[] Intersections { get; }

        public Point GetRandomPoint(Random random)
        {
            return this.Intersections[random.Next(this.Intersections.Length)];
        }

        public Point? GetRandomPointWithin(Rectangle bounds, Random random)
        {
            // todo: optimize with quadtree
            var points = this.Intersections.Where(bounds.Contains)
                .ToArray();

            if (points.Length == 0)
            {
                return null;
            }

            return points[random.Next(points.Length)];
        }
    }
}