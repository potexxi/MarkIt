using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarkIt.UserControls
{
    /// <summary>
    /// Interaktionslogik für FileBar.xaml
    /// </summary>
    public partial class FileBar : UserControl
    {
        private string selectedPath;
        private TreeViewItem? selectedTreeViewItem;
        public FileBar(List<string> history)
        {
            InitializeComponent();
            DrawHistory(history);
            UpdateFileTreeLocal();
            UpdateColors();
        }

        public void UpdateColors()
        {
            BorderMain.Background = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.BackgroundColor);
            LabelRecent.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            LabelLocal.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            LabelCloud.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
        }

        public void DrawHistory(List<string> history)
        {
            StackPanelHistory.Children.Clear();
            if (history == null || history.Count == 0)
            {
                Label label = new Label();
                label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                label.Content = "No recent files...";
                label.Margin = new Thickness(10, -5, 0, 0);
                label.FontFamily = new FontFamily("Leelawadee UI");
                label.FontSize = 14;
                StackPanelHistory.Children.Add(label);
            }
            foreach (string item in history)
            {
                Label label = new Label();
                label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                label.MouseEnter += Label_MouseEnter;
                label.MouseLeave += Label_MouseLeave;
                label.PreviewMouseDoubleClick += LabelHistory_PreviewMouseDoubleClick;
                label.PreviewMouseDown += LabelHistory_PreviewMouseDown;
                label.Margin = new Thickness(10, -5, 0, 0);
                label.Cursor = Cursors.Hand;
                label.FontFamily = new FontFamily("Leelawadee UI");
                label.FontSize = 14;
                string[] parts = item.Split("/");
                label.Content = string.Join("/" ,parts.Skip(2));
                StackPanelHistory.Children.Add(label);
            }
        }

        private void LabelHistory_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach(Label label1 in  StackPanelHistory.Children)
            {
                label1.Background = Brushes.Transparent;
            }
            Label label = (Label)sender;
            label.Background = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            selectedPath = label.Content.ToString();
            ButtonDel.IsEnabled = false;
        }

        private void LabelHistory_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)sender;
            selectedPath = label.Content.ToString();
            MainWindow.FileManager.LoadFromFile(selectedPath, false);
            if (this.Parent is Panel panel)
            {
                panel.Children.Remove(this);
            }
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
        }

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            label.Foreground = (Brush) new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.HoverColor);
        }

        private void Rectangle_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.Parent is Panel panel)
            {
                panel.Children.Remove(this);
            }
        }

        public void SetSize(double width, double height)
        {
            this.Height = height;
            this.Width = width;
            RectBackground.Height = height;
            RectBackground.Width = width;
            BorderMain.Width = 0.3 * width;
            BorderMain.Height = 0.97 * height - 50;
            ScrollViewerMain.Height = 0.78 * (0.97 * height - 50);
            ScrollViewerMain.Width = 0.3 * width;
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            if(selectedTreeViewItem != null)
            {
                if(selectedTreeViewItem.Tag.ToString() == "root")
                {
                    WindowMessageBox box1 = new WindowMessageBox("Cannot remove!", "You cannot remove this folder! It is your root folder.");
                    box1.ShowDialog();
                    return;
                }
                WindowMessageBox box;
                if (Directory.Exists(selectedTreeViewItem.Tag.ToString()))
                    box = new WindowMessageBox("Really?", "Do you really want to delete this folder? All files will be removed in this folder.", WindowMessageBox.ButtonType.YesNo);
                else
                    box = new WindowMessageBox("Really?", "Do you really want to delete this file?", WindowMessageBox.ButtonType.YesNo);
                box.ShowDialog();
                if(box.returnType == WindowMessageBox.ReturnType.Yes)
                {
                    if(!RemoveItem(TreeViewLocal, selectedTreeViewItem))
                    {
                        RemoveItem(TreeViewCloud, selectedTreeViewItem);
                    }
                    if (Directory.Exists(selectedTreeViewItem.Tag.ToString()))
                    {
                        Directory.Delete(selectedTreeViewItem.Tag.ToString(), true);
                    }
                    else if (File.Exists(selectedTreeViewItem.Tag.ToString()))
                    {
                        File.Delete(selectedTreeViewItem.Tag.ToString());
                    }
                    selectedTreeViewItem = null;
                }
            }
        }

        // ChatGPT-Anfang
        // Prompt: <Funktion ohne itemscontrol und 2. if in der for loop> wieso geht diese funktion nicht bei eine file tree view
        private bool RemoveItem(ItemsControl treeView, TreeViewItem item)
        {
            foreach(TreeViewItem item2 in treeView.Items)
            {
                if(item2 == item)
                {
                    treeView.Items.Remove(item);
                    return true;
                }
                if(item2 is TreeViewItem treeView1)
                {
                    if(RemoveItem(treeView1, item))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // ChatGPT-Ende

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            if(selectedPath != "" && selectedPath != null)
            {
                // TODO: Read file
                if (this.Parent is Panel panel)
                {
                    panel.Children.Remove(this);
                }
            }
        }

        public void UpdateFileTreeLocal()
        {
            string path = MainWindow.FileManager.userPath;
            TreeViewLocal.Items.Clear();
            DirectoryInfo userInfo = new DirectoryInfo(path);
            if (userInfo.GetFiles().Length <= 1)
            {
                Label item = new Label();
                item.Content = "No local files.";
                item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                TreeViewLocal.Items.Add(item);
                TreeViewLocal.IsHitTestVisible = false;
                return;
            }
            TreeViewLocal.Items.Add(CreateTreeViewItem(userInfo));

        }

        private TreeViewItem CreateTreeViewItem(DirectoryInfo info)
        {
            TreeViewItem item = new TreeViewItem { Header = $"📁{info.Name}" };
            item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            item.FontSize = 14;
            item.Tag = "root";
            item.PreviewMouseDown += Fileitem_PreviewMouseDown;

            foreach (DirectoryInfo underfolder in info.GetDirectories())
            {
                TreeViewItem folderitem = CreateTreeViewItem(underfolder);
                folderitem.PreviewMouseDown += Fileitem_PreviewMouseDown;
                folderitem.Tag = underfolder.FullName;
                item.Items.Add(folderitem);
            }
            foreach(FileInfo file in info.GetFiles())
            {
                if(file.Name != "file-history.json")
                {
                    TreeViewItem fileitem = new TreeViewItem { Header = file.Name };
                    fileitem.MouseDoubleClick += Fileitem_MouseDoubleClick;
                    fileitem.PreviewMouseDown += Fileitem_PreviewMouseDown;
                    fileitem.Cursor = Cursors.Hand;
                    fileitem.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                    fileitem.FontSize = 14;
                    fileitem.Tag = file.FullName;
                    item.Items.Add(fileitem);
                }
            }
            return item;
        }

        private void Fileitem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedTreeViewItem = (TreeViewItem)sender;
        }

        private void Fileitem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            MainWindow.FileManager.LoadFromFile(item.Tag.ToString(), true);
            if (this.Parent is Panel panel)
            {
                panel.Children.Remove(this);
            }
        }

        private void ScrollViewerMain_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // CHATGPT Anfang
            // Prompt: Wieso kann ich nicht scrollen, wenn ich ueber einem treeview bin wpf
            ScrollViewerMain.RaiseEvent(
                new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta)
                {
                    RoutedEvent = UIElement.MouseWheelEvent
                });

            e.Handled = true;
            // CHATGPT Ende
        }
    }
}
