using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PuzzleSolver.Converters
{
    class GroupColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is List<Color> colors && values[1] is int index)
            {
                if (index > colors.Count)
                {
                    return new SolidColorBrush(Color.FromRgb(0, 0, 0));
                }
                return new SolidColorBrush(colors[index]);
            }
            return new SolidColorBrush(Color.FromRgb(0,0,0));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
