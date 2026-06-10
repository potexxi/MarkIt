using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MarkIt.windows
{
    public partial class WindowCreateAssistent : Window
    {
        public WindowCreateAssistent()
        {
            InitializeComponent();
        }
        // Alles was nach Claude ausschaut kommt von claude, er hat nur das design
        // und das invalid folder name gemacht
        // Updates the label when switching between File and Folder
        private void RadioButton_Changed(object sender, RoutedEventArgs e)
        {
            if (LBL_Name == null) return;
            LBL_Name.Content = RB_Folder.IsChecked == true ? "Folder Name" : "File Name";
            TBL_Error.Visibility = Visibility.Collapsed;
        }

        // Validates folder name (no invalid characters)
        private async void Button_Create_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowMessageBox box;
            // Sowas kommt von Claude, hat er alles beim design mitgemacht
            string name = CT_Name.CustomContent.Trim();
            if (string.IsNullOrEmpty(name))
            {
                box = new WindowMessageBox("No Name","Please enter a name.");
                box.ShowDialog();
                return;
            }

            // Windows folder name must not contain: \ / : * ? " < > |
            if (Regex.IsMatch(name, @"[\\/:*?""<>|]"))
            {
                box = new WindowMessageBox("Characters",@"Invalid characters: \ / : * ? "" < > |");
                box.ShowDialog();
                return;
            }
            if(RB_Local.IsChecked == true)
            {
                if(RB_Folder.IsChecked == true)
                {
                    if (Directory.Exists(MainWindow.FileManager.userPath + "/" + name))
                    {
                        box = new WindowMessageBox("Exists", "This folder already exits.");
                        box.ShowDialog();
                        return;
                    }
                    else
                    {
                        Directory.CreateDirectory(MainWindow.FileManager.userPath + "/" + name);
                        this.Close();
                    }
                }
                else if(RB_File.IsChecked == true)
                {
                    if (File.Exists(MainWindow.FileManager.userPath + "/" + name))
                    {
                        box = new WindowMessageBox("Exists", "This file already exits.");
                        box.ShowDialog();
                        return;
                    }
                    else
                    {
                        File.Create(MainWindow.FileManager.userPath + "/" + name);
                        this.Close();
                    }
                }
            }
            else if(RB_Cloud.IsChecked == true)
            {
                if(RB_Folder.IsChecked==true)
                    await MainWindow.FileManager.Upload(MainWindow.FileManager.userPath + "/" + name + "/" + ".placeholder", "", LoadingScreen);
                else if(RB_File.IsChecked==true)
                    await MainWindow.FileManager.Upload(MainWindow.FileManager.userPath + "/" + name, "", LoadingScreen);
                this.Close();
            }
        }

        private void Button_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void ShowError(string message)
        {
            TBL_Error.Text = message;
            TBL_Error.Visibility = Visibility.Visible;
        }
    }
}
