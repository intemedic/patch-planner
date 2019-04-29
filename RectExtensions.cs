using System.Windows;

namespace PatchPlanner
{
    internal static class RectExtensions
    {
        public static double GetArea(this Rect rect)
        {
            return rect.Width * rect.Height;
        }
    }
}