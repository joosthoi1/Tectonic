using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows;

namespace PuzzleSolver.Helpers;

public static class UniformGridHelper
{
    public static readonly DependencyProperty BoundColumnsProperty =
        DependencyProperty.RegisterAttached(
            "BoundColumns",
            typeof(int),
            typeof(UniformGridHelper),
            new PropertyMetadata(1, OnBoundColumnsChanged));

    public static void SetBoundColumns(DependencyObject element, int value)
    {
        element.SetValue(BoundColumnsProperty, value);
    }

    public static int GetBoundColumns(DependencyObject element)
    {
        return (int)element.GetValue(BoundColumnsProperty);
    }

    private static void OnBoundColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UniformGrid grid && e.NewValue is int newColumns)
        {
            grid.Columns = newColumns;
        }
    }
}
