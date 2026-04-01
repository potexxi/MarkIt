using MarkIt.login_register;
using Serilog;
using Serilog.Core;
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
        public static Logger logger { get; private set; }
        public MainWindow()
        {
            LoggerConfiguration config = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("markit-log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5);
            logger = config.CreateLogger();

            InitializeComponent();
            WindowUserLogin window = new WindowUserLogin();
            window.ShowDialog();


            
        }
    }
}