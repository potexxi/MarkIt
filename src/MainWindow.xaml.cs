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
using System.Windows.Threading;

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

        DispatcherTimer Maintimer = new DispatcherTimer();

        public MainWindow()
        {
            Logger.Init();


            Maintimer.Interval = TimeSpan.FromMilliseconds(15);
            Maintimer.Tick += Maintimer_Tick;

            GeneralSettings = new GeneralSettings(this.ActualWidth, this.ActualHeight, true, false, "12"); // has to be infront of init
            GeneralSettings = GeneralSettings.LoadFromFile("sources/options/generalSettings.json");
            InitializeComponent();
            ServerManager = new ServerManager();
            updateSettings();
            WindowUserLogin window = new WindowUserLogin();
            window.ShowDialog();

            FileManager = new FileManager(currentUser.Email);

            // zum Testen
            ClassWorksheet CurrentWorkSheet = new ClassWorksheet(GridWorksheet);
            CurrentWorkSheet.Init();

            filebar = new FileBar(FileManager.FileHistory);
            GridMain.Children.Add(filebar);
        }

        private void Maintimer_Tick(object? sender, EventArgs e)
        {
            updateSettings();
        }

        public void updateSettings()
        {
            if (GeneralSettings.updatedColorTheme)
            {
                UC_AccountIcon.updateSettings();
                updateColorMain();
                GeneralSettings.updatedColorTheme = false;
            }
        }


        public void updateColorMain()
        {
            StackPanelNavigationBar.Background = (Brush)new BrushConverter().ConvertFromString(GeneralSettings.currentColorTheme.BackgroundColor);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            filebar.SetSize(this.ActualWidth, this.ActualHeight);
        }
    }
}