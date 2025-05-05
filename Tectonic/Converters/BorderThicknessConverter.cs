using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Tectonic.Models;

namespace Tectonic.Converters;

public class BorderThicknessConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is PositionedCelldata cell && values[1] is GameBoard grid)
        {
            bool right = grid.HasRightBorder(cell);
            bool bottom = grid.HasBottomBorder(cell);
            return new Thickness(1, 1, right ? 3 : 1, bottom ? 3 : 1);
        }

        return new Thickness(0);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
