using MarkIt.login_register;
using MarkIt.settings;
using MarkIt.UserControls;
using MarkIt.worksheet;
using Serilog;
using Serilog.Core;
using Supabase.Gotrue;
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
        public static Supabase.Client supabase;
        public static ClassUser currentUser;
        public static Session currentSession;
        public static ServerManager ServerManager;
        public static FileManager FileManager;
        public static GeneralSettings GeneralSettings;
        private FileBar filebar;
        public MainWindow()
        {
            InitializeComponent();
            Logger.Init();
            ServerManager = new ServerManager();
            GeneralSettings = new GeneralSettings(this.ActualWidth, this.ActualHeight, true, false, "12");
            GeneralSettings.SaveColorsToFile();
            ServerManager.InitSupabaseClient();
            WindowUserLogin window = new WindowUserLogin();
            window.ShowDialog();

            FileManager = new FileManager(currentUser.Email);

            // zum Testen
            ClassWorksheet CurrentWorkSheet = new ClassWorksheet(GridWorksheet);
            CurrentWorkSheet.Init();

            filebar = new FileBar(FileManager.FileHistory);
            GridMain.Children.Add(filebar);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            filebar.SetSize(this.ActualWidth, this.ActualHeight);
        }
    }
}