using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace PuzzleSolver.Converters.Border;

public class CellBorderInfo
{
    public Thickness LeftThickness { get; set; }
    public Brush LeftBrush { get; set; }
    public Thickness TopThickness { get; set; }
    public Brush TopBrush { get; set; }
    public Thickness RightThickness { get; set; }
    public Brush RightBrush { get; set; }
    public Thickness BottomThickness { get; set; }
    public Brush BottomBrush { get; set; }
}