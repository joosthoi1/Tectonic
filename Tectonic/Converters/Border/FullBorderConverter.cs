using PuzzleSolver.Models.Puzzles;
using PuzzleSolver.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;
using PuzzleSolver.Converters.Border;

namespace PuzzleSolver.Converters;

public class FullBorderConverter : IMultiValueConverter
{
    private enum BorderSide
    {
        Left,
        Top,
        Right,
        Bottom
    }
    static double _boldSize = 3;
    static double _thinSize = 0.5;
    static Brush _lightBrush = Brushes.LightGray;
    static Brush _darkBrush = Brushes.Black;
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 ||
            !(values[0] is PositionedCelldata currentCell) ||
            !(values[1] is IGameBoard board))
        {
            // Return a default/error state or null
            return new CellBorderInfo // Return default visible borders for design time or errors
            {
                LeftThickness = new Thickness(_thinSize, 0, 0, 0),
                LeftBrush = _lightBrush,
                TopThickness = new Thickness(0, _thinSize, 0, 0),
                TopBrush = _lightBrush,
                RightThickness = new Thickness(0, 0, _thinSize, 0),
                RightBrush = _lightBrush,
                BottomThickness = new Thickness(0, 0, 0, _thinSize),
                BottomBrush = _lightBrush
            };
        }

        var borderInfo = new CellBorderInfo();
        var borders = board.GetBorders(currentCell);

        string param = parameter.ToString();
        

        borderInfo.LeftThickness = borders.Left ? new Thickness(_boldSize, 0, 0, 0) : new Thickness(_thinSize, 0, 0, 0);
        borderInfo.TopThickness = borders.Top ? new Thickness(0, _boldSize, 0, 0) : new Thickness(0, _thinSize, 0, 0);
        borderInfo.RightThickness = borders.Right ? new Thickness(0, 0, _boldSize, 0) : new Thickness(0, 0, _thinSize, 0);
        borderInfo.BottomThickness = borders.Bottom ? new Thickness(0, 0, 0, _boldSize) : new Thickness(0, 0, 0, _thinSize);

        borderInfo.LeftBrush = borders.Left ? _darkBrush : _lightBrush;
        borderInfo.TopBrush = borders.Top ? _darkBrush : _lightBrush;
        borderInfo.RightBrush = borders.Right ? _darkBrush : _lightBrush;
        borderInfo.BottomBrush = borders.Bottom ? _darkBrush : _lightBrush;

        switch (param)
        {
            case "LeftThickness":
                return borderInfo.LeftThickness;

            case "TopThickness":
                return borderInfo.TopThickness;

            case "RightThickness":
                return borderInfo.RightThickness;

            case "BottomThickness":
                return borderInfo.BottomThickness;

            case "LeftBrush":
                return borderInfo.LeftBrush;

            case "TopBrush":
                return borderInfo.TopBrush;

            case "RightBrush":
                return borderInfo.RightBrush;

            case "BottomBrush":
                return borderInfo.BottomBrush;

            default:
                return DependencyProperty.UnsetValue;
        }


        return borderInfo;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
