using System;
using System.Diagnostics;
using System.Drawing;

namespace Intemedic.PatchPlanner
{
    /// <remarks>
    /// Can only represent a vertical or horizontal line segment.
    /// </remarks>
    [DebuggerDisplay("{Start} - {End}")]
    public class LineSegment
    {
        public LineSegment(Point start, Point end)
        {
            this.Start = start;
            this.End = end;
        }

        public Point Start { get; }
        public Point End { get; }

        public bool IsHorizontal => this.Start.Y == this.End.Y;
        public bool IsVertical => this.Start.X == this.End.X;

        public Point? GetIntersectionWith(LineSegment other)
        {
            if (this.IsHorizontal && other.IsHorizontal
                || this.IsVertical && other.IsVertical)
            {
                return null;
            }

            if (this.IsHorizontal)
            {
                Debug.Assert(other.IsVertical);
                var minX = Math.Min(this.Start.X, this.End.X);
                var maxX = Math.Max(this.Start.X, this.End.X);
                var otherX = other.Start.X;
                if (otherX < minX || otherX > maxX)
                {
                    return null;
                }

                var minY = Math.Min(other.Start.Y, other.End.Y);
                var maxY = Math.Max(other.Start.Y, other.End.Y);
                var thisY = this.Start.Y;
                if (thisY < minY || thisY > maxY)
                {
                    return null;
                }

                return new Point(otherX, thisY);
            }

            /*if (this.IsVertical)*/
            {
                Debug.Assert(other.IsHorizontal);

                var minX = Math.Min(other.Start.X, other.End.X);
                var maxX = Math.Max(other.Start.X, other.End.X);
                var thisX = this.Start.X;
                if (thisX < minX || thisX > maxX)
                {
                    return null;
                }

                var minY = Math.Min(this.Start.Y, this.End.Y);
                var maxY = Math.Max(this.Start.Y, this.End.Y);
                var otherY = other.Start.Y;
                if (otherY < minY || otherY > maxY)
                {
                    return null;
                }

                return new Point(thisX, otherY);
            }
        }
    }
}