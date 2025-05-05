using System;
using System.Collections.Generic;
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
            (this.DataContext as MainViewModel).LoadPuzzle("test");
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
    }
}
