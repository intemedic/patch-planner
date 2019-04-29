using System;
using System.Windows;

namespace PatchPlanner
{
    public class Patch
    {
        public static Patch CreateRandom(Random random)
        {
            var x = random.Next(Constants.CanvasSize - Constants.PatchSize);
            var y = random.Next(Constants.CanvasSize - Constants.PatchSize);
            return new Patch(new Point(x, y));
        }

        public Patch(Point position)
        {
            this.Position = position;
            this.Bounds = new Rect(position, new Size(Constants.PatchSize, Constants.PatchSize));
        }

        public Point Position { get; }
        public Rect Bounds { get; }
    }
}