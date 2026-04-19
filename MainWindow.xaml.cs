using MarkIt.login_register;
using MarkIt.worksheet;
using Serilog;
using Serilog.Core;
using System.Runtime.CompilerServices;
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

namespace MarkIt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ClassUser currentUser;
        public MainWindow()
        {
            InitializeComponent();
            Logger.Init();
            WindowUserLogin window = new WindowUserLogin();
            window.ShowDialog();

            // zum Testen
            ClassWorksheet CurrentWorkSheet = new ClassWorksheet(GridWorksheet);
            CurrentWorkSheet.Init();
        }
    }
}