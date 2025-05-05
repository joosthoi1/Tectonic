using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tectonic.Models;
using Tectonic.Providers;
using Tectonic.ViewModels;

namespace Tectonic
{
    /// <summary>
    /// Interaction logic for CreatorPage.xaml
    /// </summary>
    public partial class CreatorPage : Page
    {
        public CreatorPage()
        {
            InitializeComponent();
            (this.DataContext as MainViewModel).LoadPuzzle();
        }
        public CreatorPage(string name)
        {
            InitializeComponent();
            (this.DataContext as MainViewModel).LoadPuzzle(name);
        }
        public CreatorPage(int x, int y, string title)
        {
            InitializeComponent();
            GameBoard puzzle = new GameBoard(x, y, new int[x*y], new int[x*y], title);
            (this.DataContext as MainViewModel).LoadPuzzle(puzzle);
        }
        private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.SelectAll();
            }
        }

        private bool _isDragging = false;
        private int _selectedGroup;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Trace.WriteLine("Mouse Left Button Down");
            if (sender is FrameworkElement element && element.DataContext is PositionedCelldata cell)
            {
                Trace.WriteLine($"Selected Group: {cell.Group}");
                _isDragging = true;
                _selectedGroup = cell.Group;
                Mouse.Capture(element);
            }
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                // Get the element under the mouse pointer
                var point = e.GetPosition(this);
                var hitTestResult = VisualTreeHelper.HitTest(this, point);

                if (hitTestResult?.VisualHit is FrameworkElement element && element.DataContext is PositionedCelldata cell)
                {
                    Trace.WriteLine($"Over: {cell.Group} Dragging Group: {_selectedGroup}");
                    cell.Group = _selectedGroup;
                }
            }
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            Mouse.Capture(null);
        }
    }
}
