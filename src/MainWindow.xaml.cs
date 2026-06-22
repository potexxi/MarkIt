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
using System.Diagnostics;
using System.Collections.Generic;
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
using System.IO;
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
        public static MenuItem MenuItemFilePath;

        public static Grid loadingScreen { get; private set; }

        DispatcherTimer Maintimer = new DispatcherTimer();

        public MainWindow()
        {
            Logger.Init();
            if(GeneralSettings.LoadFromFile("sources/options/generalSettings.json") == null)
            {
                GeneralSettings = new GeneralSettings(40, 40, true, false, "10"); // has to be infront of init
                GeneralSettings.SaveToFile("sources/options/generalSettings.json");
            }
            else
            {
                GeneralSettings = GeneralSettings.LoadFromFile("sources/options/generalSettings.json");
            }
            InitializeComponent();
            MenuItemFilePath = MenuItemCurrentFilePath;
            GeneralSettings.updatedColorTheme = true;
            updateSettings();
            loadingScreen = LoadingScreen;
            Maintimer.Interval = TimeSpan.FromMilliseconds(1000);
            Maintimer.Tick += Maintimer_Tick;
            Maintimer.Start(); // für das farb theme

            ServerManager = new ServerManager();
            WindowUserLogin window = new WindowUserLogin();
            window.ShowDialog();

            FileManager = new FileManager(currentUser.Email);
            filebar.Show();
            // zum Testen

            MS_Headers.setSelection(["# Header 1", "## Header 2", "### Header 3", "#### Header 4", "##### Header 5"]);
            MS_List.setSelection(["- Unorderd", " 1. Orderd", "- [ ] Checkliste"]);
            CurrentWorkSheet = new ClassWorksheet(GridWorksheet);
            CurrentWorkSheet.RenderLines();
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

                CB_Bold.updateSettings();
                CB_Code.updateSettings();
                CB_Italic.updateSettings();
                CB_Striketrough.updateSettings();
                CB_Underline.updateSettings();

                CB_Image.updateSettings();
                CB_Link.updateSettings();
                CB_Quote.updateSettings();
                CB_Subscript.updateSettings();
                CB_Superscript.updateSettings();

                MS_tabels.updateSettings();
                MS_JSON.updateSettings();

                MS_Headers.updateSettings();
                MS_List.updateSettings();

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
            CurrentWorkSheet.ScrollViewerWorksheet.Height = this.ActualHeight-220;
            filebar.SetSize(this.ActualWidth, this.ActualHeight);
            GridWorksheet.Height = this.ActualHeight - 210;
            MenuBottom.Width = this.ActualWidth - 320;
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
            WindowUserSettings settings = new WindowUserSettings();
            settings.ShowDialog();
        }

        private void AccoundIcon_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowUserSettings settings = new WindowUserSettings();
            settings.ShowDialog();
        }

        private void MenuItemNewFile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            FileManager.CreateNewFile();
            UpdateMenuItemBottom();
        }

        private void MenuItemOpenFile_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open file";
            // Filter von CHATGPT
            openFileDialog.Filter = "Markdown (*.md)|*.md|Textdateien (*.txt)|*.txt|Alle Dateien (*.*)|*.*";
            openFileDialog.InitialDirectory = FileManager.GetAbsolutPath(FileManager.userPath);
            openFileDialog.Multiselect = false;
            bool? result = openFileDialog.ShowDialog();
            if(result == true)
            {
                FileManager.CurrentFilePath = openFileDialog.FileName;
                CurrentWorkSheet.LoadFromString(FileManager.LoadFromFile(openFileDialog.FileName));
            }
            UpdateMenuItemBottom();
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

        private void MenuItemOpen_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowUserSettings settings = new WindowUserSettings();
            settings.ShowDialog();
        }

        private void MenuItemLogout_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // ChatGPT-Anfang
            // prompt: wie kann ich eine application neustarten in c# code
            Process.Start(Environment.ProcessPath!);
            Application.Current.Shutdown();
             // ChatGPT-Ende
         }
  
        // all the Custom Button features that you can press on the navigation bar
        private void CB_Bold_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToPostion("**");}

        private void CB_Code_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToPostion("`");}

        private void CB_Italic_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToPostion("*");}

        private void CB_Striketrough_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToPostion("~~");}

        private void CB_Underline_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToPostion("<u>", "</u>");}

        private void CB_Subscript_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToPostion("<sup>", "</sup>");}

        private void CB_Superscript_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToPostion("<sub>", "</sub>");}

        private void CB_Quote_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToLineBeginning("> ");}

        private void CB_Link_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToPostion("[text](https://example.com)", "");}

        private void CB_Image_MouseDown(object sender, MouseButtonEventArgs e){CurrentWorkSheet.addToPostion("![text](image.png)", "");}

        private void MS_Headers_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string hashSTR = "#";
            for (int i = 0; i < MS_Headers.selectionIndex; i++)
                hashSTR += "#";
            hashSTR += " ";
            CurrentWorkSheet.addToLineBeginning(hashSTR);
        }

        private void MS_List_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string ListType = "- ";
            if(MS_List.selectionIndex == 1)
                ListType = "1. ";
            else if (MS_List.selectionIndex == 2)
                ListType = "- [ ] ";
            CurrentWorkSheet.addToLineBeginning(ListType);
        }

        private void MS_tabels_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CurrentWorkSheet.addTabel(MS_tabels.height, MS_tabels.width);
        }

        private void MS_tabels_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if(MS_JSON.width == 1)
                CurrentWorkSheet.addToPostion("{}", "");
            else if (MS_JSON.width == 2)
                CurrentWorkSheet.addToPostion("[{}, {}]", "");
            else if (MS_JSON.width == 3)
                CurrentWorkSheet.addToPostion("[{}, {}, {}]", "");
            else if (MS_JSON.width == 4)
                CurrentWorkSheet.addToPostion("[{}, {}, {}, {}]", "");
            else if (MS_JSON.width == 5)
                CurrentWorkSheet.addToPostion("[{}, {}, {}, {}, {}]", "");
            else if (MS_JSON.width == 6)
                CurrentWorkSheet.addToPostion("[{}, {}, {}, {}, {}, {}]", "");
            else
                CurrentWorkSheet.addToPostion("[{}, {}, {}, {}, {}, {}, {}]", "");
        }

        private async void MenuItemExport_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MenuItemExport.IsEnabled = false;
            LoadingScreenExport.Visibility = Visibility.Visible;
            await FileManager.MarkdownToPdf(CurrentWorkSheet.GetContent(), ProgressBarExport);
            LoadingScreenExport.Visibility = Visibility.Hidden;
            MenuItemExport.IsEnabled = true;
            ProgressBarExport.Value = 0;
        }

        public static void UpdateMenuItemBottom()
        {
            if(string.IsNullOrEmpty(FileManager.CurrentFilePath))
            {
                MainWindow.MenuItemFilePath.Header = "No File opened";
                MainWindow.MenuItemFilePath.IsEnabled = false;
            }
            else
            {
                MainWindow.MenuItemFilePath.Header = FileManager.CurrentFilePath;
                MainWindow.MenuItemFilePath.IsEnabled = true;
            }
        }

        private void MenuItemOpenFolder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(FileManager.CurrentFilePath))
                return;
            // ChatGPT
            // Prompt: wie kann ich process start fuer folder
            Process.Start(new ProcessStartInfo
            {
                FileName = Directory.GetParent(FileManager.CurrentFilePath).ToString(),
                UseShellExecute = true
            });
            // Ende
        }
    }
}