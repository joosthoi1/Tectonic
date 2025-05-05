using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SolverPage : Page
    {
        public SolverPage()
        {
            InitializeComponent();
            (this.DataContext as MainViewModel).LoadPuzzle("test");
        }
        public SolverPage(string name)
        {
            InitializeComponent();
            (this.DataContext as MainViewModel).LoadPuzzle(name);
        }
    }
}