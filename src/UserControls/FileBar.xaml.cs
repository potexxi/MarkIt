using MarkIt.settings;
using Supabase.Storage;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MarkIt.UserControls
{
    /// <summary>
    /// Interaktionslogik für FileBar.xaml
    /// </summary>
    public partial class FileBar : UserControl
    {
        private string selectedPath;
        private TreeViewItem? selectedTreeViewItem;
        public List<string> history;
        private DispatcherTimer timerBlurOut;
        private DispatcherTimer timerBlurIn;
        private DispatcherTimer timerUpdate;
        public FileBar()
        {
            InitializeComponent();
            timerBlurIn = new DispatcherTimer();
            timerBlurIn.Interval = TimeSpan.FromMilliseconds(1);
            timerBlurIn.Tick += TimerBlurIn_Tick;

            timerBlurOut = new DispatcherTimer();
            timerBlurOut.Interval = TimeSpan.FromMilliseconds(1);
            timerBlurOut.Tick += TimerBlurOut_Tick;

            timerUpdate = new DispatcherTimer();
            timerUpdate.Interval = TimeSpan.FromHours(6.7);
            timerUpdate.Tick += TimerUpdate_Tick;

            selectedPath = "";
            history = new List<string>();
        }

        private void TimerUpdate_Tick(object? sender, EventArgs e)
        {
            Update();
        }

        public void Update()
        {
            MainWindow.FileManager.setFileHistory();
            DrawHistory(MainWindow.FileManager.FileHistory);
            UpdateFileTreeLocal();
            UpdateFileTreeCloud();
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
                string[] parts = item.Split(MainWindow.currentUser.Email.Split("@")[0]);
                label.Content = parts[1];
                selectedPath = item;
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
            ButtonOpen_Click(sender, e);
            Hide();
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
            timerBlurIn.Stop();
            timerBlurOut.Stop();
            Hide();
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
            if(RectBackground.IsHitTestVisible == false)
                Canvas.SetLeft(BorderMain, -BorderMain.Width - 20);
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (selectedTreeViewItem != null)
                {
                    (string path, string type) = ((string, string))selectedTreeViewItem.Tag;
                    if (path == "root")
                    {
                        WindowMessageBox box1 = new WindowMessageBox("Cannot remove!", "You cannot remove this folder! It is your root folder.");
                        box1.ShowDialog();
                        return;
                    }
                    WindowMessageBox box;
                    if (Directory.Exists(path))
                        box = new WindowMessageBox("Really?", "Do you really want to delete this folder? All files will be removed in this folder.", WindowMessageBox.ButtonType.YesNo);
                    else
                        box = new WindowMessageBox("Really?", "Do you really want to delete this file?", WindowMessageBox.ButtonType.YesNo);
                    box.ShowDialog();
                    if (box.returnType == WindowMessageBox.ReturnType.Yes)
                    {
                        if (type == "local")
                        {
                            if (Directory.Exists(path))
                            {
                                try
                                {
                                    Directory.Delete(path, true);
                                }
                                catch (Exception ex)
                                {
                                    box = new WindowMessageBox("Access denied.", "This folder is read-only. MarkIt cannot delete it.");
                                    box.ShowDialog();
                                    Logger.logger.Warning($"Delete directory: {ex.Message}");
                                    return;
                                }
                            }
                            else if (File.Exists(path))
                            {
                                File.Delete(path);
                            }
                        }
                        else if (type == "cloud")
                        {
                            MainWindow.FileManager.DeleteFromServer(path, MainWindow.loadingScreen);
                        }
                        if (!RemoveItem(TreeViewLocal, selectedTreeViewItem))
                        {
                            RemoveItem(TreeViewCloud, selectedTreeViewItem);
                        }
                        selectedTreeViewItem = null;
                    }
                }
            }
            catch(Exception ex)
            {
                WindowMessageBox box = new WindowMessageBox("Unexpected Error!", "This action caused a unexpected error, please try again.");
                box.ShowDialog();
                Logger.logger.Error($"Delete file: {ex.Message}");
            }

        }

        // ChatGPT-Anfang
        // Prompt: <Funktion ohne itemscontrol und 2. if in der for loop> wieso geht diese funktion nicht bei eine file tree view
        private bool RemoveItem(ItemsControl treeView, TreeViewItem item)
        {
            selectedTreeViewItem = null;
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

        private async void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string path;
                string type;
                if (selectedTreeViewItem != null)
                {
                    (path, type) = ((string, string))selectedTreeViewItem.Tag;
                }
                else if (selectedPath != "")
                {
                    path = selectedPath;
                    type = "local";
                }
                else { return; }
                if (type == "local")
                {
                    if (!File.Exists(path))
                    {
                        RemoveItem(TreeViewLocal, selectedTreeViewItem);
                        WindowMessageBox box = new WindowMessageBox("File does not exist.", "This file does not exist anymore.");
                        box.ShowDialog();
                        return;
                    }
                    MainWindow.CurrentWorkSheet.LoadFromString(MainWindow.FileManager.LoadFromFile(path));
                }
                else
                {
                    MainWindow.CurrentWorkSheet.LoadFromString(await MainWindow.FileManager.Download(path, MainWindow.loadingScreen));
                }
                Hide();
                selectedTreeViewItem = null;
                selectedPath = "";
            }
            catch(Exception ex)
            {

            }
        }

        public async Task UpdateFileTreeCloud()
        {
            TreeViewCloud.IsHitTestVisible = true;
            TreeViewCloud.Items.Clear();
            if(MainWindow.currentUser.Email != "guest")
            {
                TreeViewItem item = await GetAllCloudPath(MainWindow.FileManager.userPath);
                TreeViewCloud.Items.Add(item);
            }
            else
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = "You need to be logged in, \nto use this function.";
                item.FontSize = 14;
                item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                TreeViewCloud.IsHitTestVisible = false;
                TreeViewCloud.Items.Add(item);
            }
        }

        public void UpdateFileTreeLocal()
        {
            TreeViewLocal.IsHitTestVisible = true;
            string path = MainWindow.FileManager.userPath;
            TreeViewLocal.Items.Clear();
            DirectoryInfo userInfo = new DirectoryInfo(path);
            if (userInfo.GetFiles().Length <= 1)
            {
                Label item = new Label();
                item.Content = "No local files.";
                item.FontSize = 14;
                item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                TreeViewLocal.Items.Add(item);
                TreeViewLocal.IsHitTestVisible = false;
                return;
            }
            TreeViewLocal.Items.Add(GetAllLocalPath(userInfo));

        }

        private TreeViewItem GetAllLocalPath(DirectoryInfo info)
        {
            TreeViewItem item = new TreeViewItem { Header = $"📁{info.Name}" };
            item.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
            item.FontSize = 14;
            item.Tag = (info.Name, "local");
            item.PreviewMouseDown += Fileitem_PreviewMouseDown;

            foreach (DirectoryInfo underfolder in info.GetDirectories())
            {
                TreeViewItem folderitem = GetAllLocalPath(underfolder);
                folderitem.PreviewMouseDown += Fileitem_PreviewMouseDown;
                folderitem.Tag = (underfolder.FullName, "local");
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
                    fileitem.Tag = (file.FullName, "local");
                    item.Items.Add(fileitem);
                }
            }
            return item;
        }

        public async Task<TreeViewItem> GetAllCloudPath(string userpath)
        {
            TreeViewItem big = new TreeViewItem();
            big.Tag = userpath;
            big.FontSize = 14;
            big.Header = $"📁 {userpath.Split('/')[^1]}";
            big.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);

            List<FileObject>? storageitems = await MainWindow.supabase.Storage.From("MarkIt").List(userpath);
            if (storageitems == null || storageitems.Count == 0)
            {
                big.Header = "No cloud files.";
                TreeViewCloud.IsHitTestVisible = false;
                return big;
            }

            foreach (FileObject item in storageitems)
            {
                if (item.IsFolder)
                {
                    TreeViewItem underfolder = await GetAllCloudPath($"{userpath}/{item.Name}");
                    underfolder.FontSize = 14;
                    underfolder.Tag = (underfolder.Name, "cloud");
                    underfolder.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                    big.Items.Add(underfolder);
                }
                else
                {
                    TreeViewItem small = new TreeViewItem();
                    small.Tag = ($"{userpath}/{item.Name}", "cloud");
                    small.Header = item.Name;
                    small.Foreground = (Brush)new BrushConverter().ConvertFromString(MainWindow.GeneralSettings.currentColorTheme.Foreground);
                    small.Cursor = Cursors.Hand;
                    small.PreviewMouseDown += Fileitem_PreviewMouseDown;
                    small.PreviewMouseDoubleClick += Fileitem_MouseDoubleClick;
                    small.FontSize = 14;
                    big.Items.Add(small);
                }
            }
            return big;
        }

        private void Fileitem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            selectedTreeViewItem = (TreeViewItem)sender;
        }


        public void Show()
        {
            Logger.logger.Debug("Open file-workspace.");
            Update();
            timerUpdate.Start();
            Canvas.SetLeft(BorderMain, 10);
            if (MainWindow.GeneralSettings.iconAnimations)
                timerBlurIn.Start();
            else
                RectBackground.Opacity = 0.5;
            RectBackground.IsHitTestVisible = true;
            // Valentin-GPT
            // prompt: wie hast du das gemacht bei raumwechsel
            DoubleAnimation animation = new DoubleAnimation
            {
                To = 13, // Zielposition
                Duration = TimeSpan.FromMilliseconds(400),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };
            BorderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            // Ende
        }

        public void Hide()
        {
            timerUpdate.Stop();
            if (MainWindow.GeneralSettings.iconAnimations)
                timerBlurOut.Start();
            else
                RectBackground.Opacity = 0;
            RectBackground.IsHitTestVisible = false;
            // Valentin-GPT
            // prompt: wie hast du das gemacht bei raumwechsel
            DoubleAnimation animation = new DoubleAnimation
            {
                To = -BorderMain.Width - 20, // Zielposition
                Duration = TimeSpan.FromMilliseconds(400),
                EasingFunction = new CubicEase
                {
                    EasingMode = EasingMode.EaseOut
                }
            };
            BorderTransform.BeginAnimation(TranslateTransform.XProperty, animation);
            // Ende
        }

        private void TimerBlurIn_Tick(object? sender, EventArgs e)
        {
            RectBackground.Opacity += 0.05;
            if(RectBackground.Opacity >= 0.5)
            {
                timerBlurIn.Stop();
            }
        }

        private void TimerBlurOut_Tick(object? sender, EventArgs e)
        {
            RectBackground.Opacity -= 0.05;
            if (RectBackground.Opacity <= 0)
            {
                timerBlurOut.Stop();
            }
        }

        private void Fileitem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ButtonOpen_Click(sender, e);
            Hide();
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
