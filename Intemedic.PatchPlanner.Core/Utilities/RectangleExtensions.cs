using System.Drawing;

namespace Intemedic.PatchPlanner.Utilities
{
    internal static class RectangleExtensions
    {
        public static int GetArea(this Rectangle rect)
        {
            return rect.Width * rect.Height;
        }
    }
}