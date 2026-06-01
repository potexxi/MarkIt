using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MarkIt.windows
{
    /// <summary>
    /// Interaktionslogik für WindowRename.xaml
    /// </summary>
    public partial class WindowRename : Window
    {
        private string path {  get; init; }
        private FileManager.FileType type {  get; init; }
        public WindowRename(string path, FileManager.FileType type)
        {
            InitializeComponent();
            this.path = path;
            this.type = type;
        }

        private void CustomButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private async void CustomButton_PreviewMouseDown_1(object sender, MouseButtonEventArgs e)
        {
            WindowMessageBox box;
            // Sowas kommt von Claude, hat er alles beim design mitgemacht
            // Kopiert vom FileAssistent
            string name = CT_Name.CustomContent.Trim();
            if (string.IsNullOrEmpty(name))
            {
                box = new WindowMessageBox("No Name", "Please enter a name.");
                box.ShowDialog();
                return;
            }

            // Windows folder name must not contain: \ / : * ? " < > |
            if (Regex.IsMatch(name, @"[\\/:*?""<>|]"))
            {
                box = new WindowMessageBox("Characters", @"Invalid characters: \ / : * ? "" < > |");
                box.ShowDialog();
                return;
            }
            if(type == FileManager.FileType.Local || (type == FileManager.FileType.Root && System.IO.Path.IsPathRooted(path)))
            {
                if (File.Exists(path))
                {
                    string newPath = System.IO.Path.GetDirectoryName(path);
                    File.Move(path, newPath + "/" + name);
                    this.Close();
                }
                else if (Directory.Exists(path))
                {
                    string newPath = Directory.GetParent(path).FullName;
                    Directory.Move(path, newPath + "/" + name);
                    this.Close();
                }
            }
            else if (type == FileManager.FileType.Cloud || (type == FileManager.FileType.Root && !System.IO.Path.IsPathRooted(path)))
            {
                // ChatGPT-Anfang
                // Prompt: <code davor> wieso geht diese Funktion nicht in supabase files und folder renamen
                try 
                {
                    await MainWindow.supabase.Storage.From("MarkIt").Download(path, null);
                    string dir = System.IO.Path.GetDirectoryName(path)?.Replace("\\", "/") ?? "";
                    string newPath = string.IsNullOrEmpty(dir) ? name : $"{dir}/{name}";

                    await MainWindow.supabase.Storage.From("MarkIt").Move(path, newPath);

                    this.Close();
                    return;
                }
                catch
                {
                    var files = await MainWindow.supabase.Storage.From("MarkIt").List(path);
                    foreach (var file in files)
                    {
                        string oldFilePath = $"{path}/{file.Name}".Replace("//", "/");
                        string newFilePath = $"{MainWindow.FileManager.userPath}/{name}/{file.Name}".Replace("//", "/");
                        await MainWindow.supabase.Storage.From("MarkIt").Move(oldFilePath, newFilePath);
                    }
                    // ChatGPT-Ende
                    this.Close();
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                CustomButton_PreviewMouseDown_1(sender, null);
            }
        }
    }
}
