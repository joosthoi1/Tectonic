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

namespace Tectonic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static NavigationService AppNavigationService { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            AppNavigationService = MainFrame.NavigationService;
            MainFrame.Navigate(new MainPage());
        }
    }
}