using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using PuzzleSolver.Models;
using PuzzleSolver.Models.Puzzles;

namespace PuzzleSolver.Converters;

public class BorderThicknessConverter : IMultiValueConverter
{
    static double boldSize = 3;
    static double thinSize = 0.5;
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is PositionedCelldata cell && values[1] is IGameBoard grid)
        {
            var (top, right, bottom, left) = grid.GetBorders(cell);
            return new Thickness(
                left ? boldSize : thinSize,
                top ? boldSize : thinSize,
                right ? boldSize : thinSize,
                bottom ? boldSize : thinSize
                );
        }

        return new Thickness(0);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
