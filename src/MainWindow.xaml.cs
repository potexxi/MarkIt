using MarkIt.login_register;
using MarkIt.settings;
using MarkIt.UserControls;
using MarkIt.windows;
using MarkIt.worksheet;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.Win32;
using Serilog;
using Serilog.Core;
using Supabase.Gotrue;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
        public static ClassWorksheet CurrentWorkSheet;
        public static Grid loadingScreen { get; private set; }

        DispatcherTimer Maintimer = new DispatcherTimer();

        public MainWindow()
        {
            Logger.Init();
            GeneralSettings = new GeneralSettings(this.ActualWidth, this.ActualHeight, true, false, "12"); // has to be infront of init
            GeneralSettings = GeneralSettings.LoadFromFile("sources/options/generalSettings.json");
            InitializeComponent();
            loadingScreen = LoadingScreen;
            Maintimer.Interval = TimeSpan.FromMilliseconds(1000);
            Maintimer.Tick += Maintimer_Tick;
            Maintimer.Start(); // für das farb theme

            ServerManager = new ServerManager();
            updateSettings();
            WindowUserLogin window = new WindowUserLogin();
            window.ShowDialog();

            FileManager = new FileManager(currentUser.Email);
            filebar.Show();
            // zum Testen
            CurrentWorkSheet = new ClassWorksheet(GridWorksheet);
            CurrentWorkSheet.Init();
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
                UC_Settings.updateSettings();
                UC_Information.updateSettings();
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

        private void MenuItemWorkspace_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            filebar.Show();
        }

        private void MenuItemExit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Logger.logger.Information("Closed application.");
            Environment.Exit(0);
        }

        private void InformationIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Logger.logger.Debug("Open InfoTab");
            WindowInfoTab info = new WindowInfoTab();
            info.ShowDialog();
        }

        private void MenuItemCredits_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            InformationIcon_PreviewMouseDown(sender, e);
        }

        private void MenuItemGeneralSettings_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowSettings settings = new WindowSettings();
            settings.ShowDialog();
        }

        private void MenuItemUserSettings_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void AccoundIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuItemNewFile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            FileManager.CreateNewFile();
        }

        private void MenuItemOpenFile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open file";
            // Filter von CHATGPT
            openFileDialog.Filter = "Markdown (*.md)|*.md|Textdateien (*.txt)|*.txt|Alle Dateien (*.*)|*.*";
            bool? result = openFileDialog.ShowDialog();
            if(result == true)
            {
                FileManager.CurrentFilePath = openFileDialog.FileName;
                CurrentWorkSheet.LoadFromString(FileManager.LoadFromFile(openFileDialog.FileName));
            }
        }

        private void MenuItemSaveFile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (FileManager.fileType == FileManager.FileType.Local)
                FileManager.SaveToFile(FileManager.CurrentFilePath, CurrentWorkSheet.GetContent());
            else if (FileManager.fileType == FileManager.FileType.Cloud)
                FileManager.Upload(FileManager.CurrentFilePath, CurrentWorkSheet.GetContent(), LoadingScreen);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                MenuItemSaveFile_PreviewMouseDown(null, null);
            }
        }
    }
}