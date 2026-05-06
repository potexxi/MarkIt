using MarkIt.login_register;
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
        public MainWindow()
        {
            InitializeComponent();
            Logger.Init();
            ServerManager = new ServerManager();
            var colorthemeBlue = new ColorTheme("blue", "#FF01021C", "#97D5C8", "#FFEA00", "#FF1F4572", "#97D5C8", "#FFFFFF");
            //GeneralSettings = new GeneralSettings(this.ActualWidth, this.ActualHeight, colorthemeBlue);
            ServerManager.InitSupabaseClient();
            WindowUserLogin window = new WindowUserLogin();
            window.ShowDialog();

            FileManager = new FileManager(currentUser.Email);

            FileManager.SaveToFile("test/test/test/test6.json", "adsfd");

            // zum Testen
            ClassWorksheet CurrentWorkSheet = new ClassWorksheet(GridWorksheet);
            CurrentWorkSheet.Init();

            var uc = new FileBar(FileManager.FileHistory, colorthemeBlue);
            GridMain.Children.Add(uc);
        }
    }
}