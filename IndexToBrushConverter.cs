using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PatchPlanner
{
    public class IndexToBrushConverter : IValueConverter
    {
        private static readonly Random Random = new Random();

        private static Dictionary<int, Brush> BrushMap { get; }
            = new Dictionary<int, Brush>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var index = (int) value;

            if (BrushMap.TryGetValue(index, out var brush))
            {
                return brush;
            }

            brush = new SolidColorBrush(
                Color.FromArgb(
                    0xff,
                    (byte) Random.Next(256),
                    (byte) Random.Next(256),
                    (byte) Random.Next(256)));

            brush.Freeze();

            BrushMap[index] = brush;

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}